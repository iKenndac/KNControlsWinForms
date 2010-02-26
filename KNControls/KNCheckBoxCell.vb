Public Class KNCheckBoxCell
    Inherits KNActionCell

    Private _isMouseOver As Boolean = False
    Private _isMouseDown As Boolean = False

    Private _preClickValue As KNCellState



    Public Overrides Function NewInstance() As KNCell

        Return New KNCheckBoxCell

    End Function

    Public Overrides Sub DrawInFrame(ByRef g As Graphics, ByRef frame As Rectangle)

        ' Checks the state and draws the correct one

        Dim size As Size = CheckBoxRenderer.GetGlyphSize(g, VisualStyles.CheckBoxState.UncheckedNormal)

        Dim boxPoint = New Point(frame.X + (frame.Width / 2) - (size.Width / 2), frame.Y + (frame.Height / 2) - (size.Height / 2))

        Dim state As VisualStyles.CheckBoxState


        If TypeOf (Me.ObjectValue) Is Boolean Then

            If CType(MyBase.ObjectValue, Boolean) = True Then
                MyBase.State = KNCellState.KNOnState
            Else
                MyBase.State = KNCellState.KNOffState
            End If

        Else
            MyBase.State = KNCellState.KNOffState

        End If


        If _isMouseDown Then
            If _isMouseOver Then
                If MyBase.State = KNCellState.KNOnState Then
                    state = VisualStyles.CheckBoxState.CheckedPressed
                Else
                    state = VisualStyles.CheckBoxState.UncheckedPressed
                End If
            Else
                If MyBase.State = KNCellState.KNOnState Then
                    state = VisualStyles.CheckBoxState.CheckedNormal
                Else
                    state = VisualStyles.CheckBoxState.UncheckedNormal
                End If
            End If
        Else
            If _isMouseOver Then
                If MyBase.State = KNCellState.KNOnState Then
                    state = VisualStyles.CheckBoxState.CheckedHot
                Else
                    state = VisualStyles.CheckBoxState.UncheckedHot
                End If
            Else
                If MyBase.State = KNCellState.KNOnState Then
                    state = VisualStyles.CheckBoxState.CheckedNormal
                Else
                    state = VisualStyles.CheckBoxState.UncheckedNormal
                End If

            End If
        End If

        CheckBoxRenderer.DrawCheckBox(g, boxPoint, state)



    End Sub

    Private Function GlyphRectangleForFrame(ByVal frame As Rectangle, ByRef g As Graphics) As Rectangle

        ' This method calculates the location of the glyph (checkbox box) in the given frame (i.e., the middle).

        Dim size As Size
        If Not g Is Nothing Then
            size = CheckBoxRenderer.GetGlyphSize(g, VisualStyles.CheckBoxState.UncheckedNormal)
        Else
            size = CheckBoxRenderer.GetGlyphSize(Graphics.FromImage(New Bitmap(1, 1)), VisualStyles.CheckBoxState.UncheckedNormal)
        End If

        Dim boxPoint = New Point(frame.X + (frame.Width / 2) - (size.Width / 2), frame.Y + (frame.Height / 2) - (size.Height / 2))

        Return New Rectangle(boxPoint, size)

    End Function

    ' Mouse

    Public Overrides Function MouseDidMoveInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this method is whether the cell should be redrawn or not. 

        If GlyphRectangleForFrame(relativeFrame, Nothing).Contains(relativePoint) Then
            If Not _isMouseOver Then
                _isMouseOver = True
                Return True
            End If
        Else
            If _isMouseOver Then
                _isMouseOver = False
                Return True
            End If
        End If

        Return False
    End Function

    Public Overrides Function MouseDownInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this method is whether the cell claims ownership of the following drag and mouseup events.
        ' The cell is always redrawn after this function is called.

        _isMouseDown = True

        If GlyphRectangleForFrame(relativeFrame, Nothing).Contains(relativePoint) Then
            _isMouseOver = True
            _preClickValue = MyBase.State
            ' MsgBox("Setting click status to " & _preClickValue)
            Return True
        Else
            Return False
        End If

    End Function

    Public Overrides Function MouseDraggedInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this is whether the cell should be redrawn or not. 
        ' Note: This only gets called if the cell claimed ownership of the mouse operation in MouseDownInCell. 

        _isMouseOver = GlyphRectangleForFrame(relativeFrame, Nothing).Contains(relativePoint)


        Return True
    End Function

    Public Overrides Function MouseUpInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this method is whether the cell should be redrawn or not. 
        ' Note: This only gets called if the cell claimed ownership of the mouse operation in MouseDownInCell. 

        _isMouseDown = False

        If GlyphRectangleForFrame(relativeFrame, Nothing).Contains(relativePoint) Then
            _isMouseOver = True
        End If

        If _isMouseOver = True Then
            ' Fire an action somehow

            If _preClickValue = KNCellState.KNOffState Then
                MyBase.State = KNCellState.KNOnState
                MyBase.ObjectValue = True
            Else
                MyBase.State = KNCellState.KNOffState
                MyBase.ObjectValue = False
            End If

            If Not CellDelegate Is Nothing Then
                CellDelegate.CellPerformedAction(Me)
            End If


        End If

        Return True
    End Function



End Class
