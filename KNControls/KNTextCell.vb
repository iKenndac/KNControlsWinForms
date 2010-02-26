Public Class KNTextCell
    Inherits KNEditableCell

    Private _textFont As Font = New Font("Tahoma", 8.25)
    Private _alignment As StringAlignment = StringAlignment.Near
    Private _lastKnownFrame As Rectangle
    Private _isMouseOver As Boolean
    Private _isMouseDown As Boolean
    Private _isEditable As Boolean

    Private _highlightColor As Color = Color.White
    Private _textColor As Color = Color.Black

    Public Overrides Sub DrawInFrame(ByRef g As Graphics, ByRef frame As Rectangle)

        If Not frame = _lastKnownFrame Then
            If ReferenceEquals(CurrentEditingCell, Me) Then
                AdjustEditorIfNeeded(frame)
            End If

            _lastKnownFrame = frame

        End If

        If (TypeOf (Me.ObjectValue) Is String) Then

            Dim height As Integer = g.MeasureString(CStr(Me.ObjectValue), _textFont, New SizeF(0, frame.Height)).Height

            Dim textRect As Rectangle = New Rectangle(frame.X + 5, (frame.Y + (frame.Height) / 2) - (height / 2), frame.Width - 10, frame.Height)

            Dim format As StringFormat = New StringFormat(StringFormatFlags.NoWrap)
            format.Alignment = _alignment
            format.Trimming = StringTrimming.EllipsisCharacter

            If (Me.Highlighted) Then
                g.DrawString(CStr(Me.ObjectValue), _textFont, New SolidBrush(_highlightColor), textRect, format)
            Else
                g.DrawString(CStr(Me.ObjectValue), _textFont, New SolidBrush(_textColor), textRect, format)
            End If

        End If

    End Sub

    Public Overrides Function MouseDidMoveInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this method is whether the cell should be redrawn or not. 

        If (relativeFrame).Contains(relativePoint) Then
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

        'MsgBox("Down")

        If (relativeFrame).Contains(relativePoint) Then
            _isMouseOver = True
            '_preClickValue = MyBase.State
            ' MsgBox("Setting click status to " & _preClickValue)

            ' Begin editing!
            If (IsEditable AndAlso Me.Highlighted) Then


                If Not CurrentEditingCell Is Nothing Then
                    CurrentEditingCell.EndTextEditing()
                End If

                SharedTextEditor.Font = Me.TextFont
                Me.BeginTextEditingInFrame(_lastKnownFrame)

                Return True
            Else
                Return False

            End If


        Else
            Return False
        End If

    End Function

    Public Overrides Function MouseDraggedInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this is whether the cell should be redrawn or not. 
        ' Note: This only gets called if the cell claimed ownership of the mouse operation in MouseDownInCell. 

        _isMouseOver = (relativeFrame).Contains(relativePoint)

        'Dim data As DataObject = New DataObject
        'data.SetData(DataFormats.Serializable, False, Me.ObjectValue)




        Return True
    End Function

    Public Overrides Function MouseUpInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this method is whether the cell should be redrawn or not. 
        ' Note: This only gets called if the cell claimed ownership of the mouse operation in MouseDownInCell. 

        _isMouseDown = False

        If (relativeFrame).Contains(relativePoint) Then
            _isMouseOver = True
        End If

        If _isMouseOver = True Then
            ' Fire an action somehow

            If Not CellDelegate Is Nothing Then
                CellDelegate.CellPerformedAction(Me)
            End If


        End If

        Return True
    End Function

    Public Function PreferredWidth(Optional ByRef g As Graphics = Nothing) As Integer
        Dim gr As Graphics
        If Not g Is Nothing Then
            gr = g
        Else
            gr = Graphics.FromImage(New Bitmap(1, 1))

        End If
        Return gr.MeasureString(Me.ObjectValue, Me.TextFont).Width + 12


    End Function

    Public Sub StartEditing()
        Me.BeginTextEditingInFrame(_lastKnownFrame)
    End Sub

#Region "Editing"

    Public Overrides Sub ValueDidChange(ByVal value As Object)

        'MsgBox("Changed")

        MyBase.ValueDidChange(value)

        'If Not CellDelegate Is Nothing Then
        'CellDelegate.CellPerformedAction(Me)
        'End If

        Me.ParentControl.UpdateCell(Me)
        'SharedTextEditor.Font = Me.TextFont
        'Me.AdjustEditorIfNeeded(_lastKnownFrame)


    End Sub

    Public Overrides Sub KeyWasPressed(ByVal keyChar As Char)
        MyBase.KeyWasPressed(keyChar)

        If AscW(keyChar) = Keys.Back Then

            If SharedTextEditor.SelectionStart = 0 And SharedTextEditor.SelectionLength = 0 Then
                'MsgBox("Delete was pressed at the beginning!")

                Dim notification As New KNNotification("KNTextCell_BackspaceWasPressedAtBeginningOfField", Me, Nothing)

                KNNotificationCentre.DefaultNotificationCentre.PostNotification(notification)


            End If


        End If

    End Sub

    

#End Region


    Public Property TextFont() As Font
        Get
            Return _textFont
        End Get
        Set(ByVal value As Font)
            _textFont = value
        End Set
    End Property

    Public Property Alignment() As StringAlignment
        Get
            Return _alignment
        End Get
        Set(ByVal value As StringAlignment)
            _alignment = value
        End Set
    End Property

    Public Overrides Function NewInstance() As KNCell

        Dim cell As KNTextCell = New KNTextCell
        cell.TextFont = Me.TextFont
        cell.Alignment = Me.Alignment
        cell.IsEditable = Me.IsEditable
        cell.TextColor = Me.TextColor
        cell.TextHighlightColor = Me.TextHighlightColor

        Return cell

    End Function

    Public Property TextColor() As Color
        Get
            Return _textColor
        End Get
        Set(ByVal value As Color)
            _textColor = value
        End Set
    End Property

    Public Property TextHighlightColor() As Color
        Get
            Return _highlightColor
        End Get
        Set(ByVal value As Color)
            _highlightColor = value
        End Set
    End Property

    Public Property IsEditable() As Boolean
        Get
            Return _isEditable
        End Get
        Set(ByVal value As Boolean)
            _isEditable = value
        End Set
    End Property

End Class
