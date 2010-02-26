Public Class KNRatingCell
    Inherits KNActionCell


    Private star As Bitmap = New Bitmap(Me.GetType(), "RatingStar.png")
    Private _editable As Boolean
    Dim _isDragging As Boolean
    Dim _draggingValue As Integer

    Public Sub New(Optional ByVal canEdit As Boolean = False)

        _editable = canEdit


    End Sub


    Public Overrides Sub DrawInFrame(ByRef g As Graphics, ByRef frame As Rectangle)

        If (TypeOf (Me.ObjectValue) Is Integer) Then

            Dim starCount As Integer

            If (_isDragging) Then
                starCount = _draggingValue / 20
            Else
                starCount = CInt(Me.ObjectValue) / 20
            End If

            Dim i = 1
            Dim starX As Integer = frame.X + 5
            Dim starY As Integer = frame.Y + (frame.Height / 2) - (star.Height / 2)


            For i = 1 To 5

                Dim starRect As Rectangle = New Rectangle(starX, starY, star.Width, star.Height)

                If i <= starCount Then

                    g.DrawImage(star, starRect)

                Else

                    If Editable Then
                        Dim circleRect As Rectangle = New Rectangle(starRect.X + 4, starRect.Y + 4, starRect.Width - 8, starRect.Height - 8)
                        g.DrawEllipse(Pens.DarkGray, circleRect)
                    End If

                End If


                starX += star.Width + 2
            Next




        End If

    End Sub


    Public Overrides Function MouseDownInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this method is whether the cell claims ownership of the following drag and mouseup events.
        ' The cell is always redrawn after this function is called.

        If Editable Then

            _isDragging = True

            Dim Xpixels As Integer = relativePoint.X - 5
            Dim newRating As Integer = Xpixels / star.Width

            _draggingValue = newRating * 20

            If _draggingValue > 100 Then
                _draggingValue = 100
            End If

            Return True
        Else
            Return False
        End If

    End Function

    Public Overrides Function MouseDraggedInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this is whether the cell should be redrawn or not. 
        ' Note: This only gets called if the cell claimed ownership of the mouse operation in MouseDownInCell. 

        If ContextuallyContains(relativeFrame, relativePoint) Then


            Dim Xpixels As Integer = relativePoint.X - 5
            Dim newRating As Integer = Xpixels / star.Width

            _draggingValue = newRating * 20

            If _draggingValue > 100 Then _draggingValue = 100
            If _draggingValue < 0 Then _draggingValue = 0


        Else
            _draggingValue = Me.ObjectValue
        End If



        Return True
    End Function

    Public Overrides Function MouseUpInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this method is whether the cell should be redrawn or not. 
        ' Note: This only gets called if the cell claimed ownership of the mouse operation in MouseDownInCell. 

        _isDragging = False

        If ContextuallyContains(relativeFrame, relativePoint) Then

            Dim Xpixels As Integer = relativePoint.X - 5
            Dim newRating As Integer = Xpixels / star.Width

            If newRating < 0 Then newRating = 0
            If newRating > 5 Then newRating = 5

            Me.ObjectValue = newRating * 20

            If Not CellDelegate Is Nothing Then
                CellDelegate.CellPerformedAction(Me)
            End If


        End If

        Return True
    End Function


    Private Function ContextuallyContains(ByVal rect As Rectangle, ByVal point As Point)
        If point.Y < rect.Y Or point.Y > rect.Y + rect.Height Then
            Return False
        Else
            Return True
        End If
    End Function


    Public Overrides Function NewInstance() As KNCell

        Return New KNRatingCell(Me.Editable)

    End Function

    Public Property Editable() As Boolean
        Get
            Return _editable
        End Get
        Set(ByVal value As Boolean)
            _editable = value
        End Set
    End Property

End Class
