Public Class KNTokenCell
    Inherits KNActionCell

    Private _font As Font = New Font("Tahoma", 11.0, FontStyle.Regular, GraphicsUnit.Pixel)
    Private _lastKnownDrawFrame As Rectangle
    Private _triBounds As Rectangle
    Private _isMouseOver As Boolean
    Private _isMouseDown As Boolean
    Private _allowsCopy As Boolean = False
    
    Public Property Font() As Font
        Get
            Return _font
        End Get
        Set(ByVal value As Font)
            _font = value
        End Set
    End Property

    Public Property AllowsCopy() As Boolean
        Get
            Return _allowsCopy
        End Get
        Set(ByVal value As Boolean)
            _allowsCopy = value
        End Set
    End Property

    Public Overrides Sub DrawInFrame(ByRef g As Graphics, ByRef frame As Rectangle)

        _lastKnownDrawFrame = frame

        If TypeOf (Me.ObjectValue) Is TokenizableObject Then

            Dim tokenObject As TokenizableObject = Me.ObjectValue

            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            Dim tokenBounds As Rectangle = Me.TokenRectForFrame(frame)
            Dim textSize As SizeF = g.MeasureString(tokenObject.TokenDisplayString, Me.Font)
            Dim textBounds As Rectangle = New Rectangle(tokenBounds.X, tokenBounds.Y, textSize.Width, textSize.Height)

            'If (Me.Height * 2 + _textBounds.Width) Mod 2 = 1 Then
            'Me.Width = CInt(Me.Height * 2 + _textBounds.Width + 1)
            'Else
            'Me.Width = CInt(Me.Height * 2 + _textBounds.Width)
            'End If

            Dim p As New System.Drawing.Drawing2D.GraphicsPath()

            p.StartFigure()
            p.AddArc(tokenBounds.X, tokenBounds.Y, tokenBounds.Height - 1, tokenBounds.Height - 1, 90, 180)
            p.AddLine(tokenBounds.X + tokenBounds.Height - 1, tokenBounds.Y, tokenBounds.X + tokenBounds.Width - (tokenBounds.Height - 2 * 2) - 1, tokenBounds.Y)
            p.AddArc(tokenBounds.X + tokenBounds.Width - (tokenBounds.Height - 1 * 2) - 2, tokenBounds.Y, tokenBounds.Height - 1, tokenBounds.Height - 1, 270, 180)
            p.AddLine(tokenBounds.X + tokenBounds.Width - (tokenBounds.Height - 1 * 2) - 1, tokenBounds.Y + tokenBounds.Height - 1, tokenBounds.X + tokenBounds.Height - 1, tokenBounds.Y + tokenBounds.Height - 1)
            p.CloseFigure()

            Dim brFill As SolidBrush
            Dim PenOutline As Pen
            If _isMouseOver Then
                brFill = New SolidBrush(Color.FromArgb(186, 206, 240))
                PenOutline = New Pen(Color.FromArgb(107, 149, 222))
            Else
                brFill = New SolidBrush(Color.FromArgb(222, 231, 247))
                PenOutline = New Pen(Color.FromArgb(163, 189, 235))
            End If

            g.FillPath(brFill, p)
            g.DrawPath(PenOutline, p)

            'If _tokenMode = kTokenMode.kDefaultMode Then
            If _isMouseOver And tokenObject.HasMenu Then
                _triBounds = New Rectangle(tokenBounds.X + tokenBounds.Width - 14, CInt(tokenBounds.Y + tokenBounds.Height / 2) - 3, 9, 7)
                Dim tri As New System.Drawing.Drawing2D.GraphicsPath
                tri.StartFigure()
                tri.AddLine(_triBounds.Left, _triBounds.Top, _triBounds.Left + _triBounds.Width, _triBounds.Top)
                tri.AddLine(_triBounds.Left + _triBounds.Width, _triBounds.Top, CInt(Math.Ceiling(_triBounds.Left + (_triBounds.Width / 2))), _triBounds.Top + _triBounds.Height - 1)
                tri.AddLine(CInt(_triBounds.Left + (_triBounds.Width / 2)), _triBounds.Top + _triBounds.Height - 1, _triBounds.Left, _triBounds.Top)

                g.FillPath(Brushes.White, tri)
                g.DrawPath(PenOutline, tri)
            End If
            'Else
            'in the palette - no triangle
            ' End If

            g.DrawString(tokenObject.TokenDisplayString, Me.Font, New SolidBrush(Color.Black), tokenBounds.X + CInt(tokenBounds.Width / 2) - CInt(textBounds.Width / 2), _
                        tokenBounds.Y + Math.Floor(CInt(tokenBounds.Height / 2) - CInt(textBounds.Height / 2)))
            'If Me.Focused And Me.TokenMode = kTokenMode.kPaletteMode Then
            'draw focus rectangle
            'Dim selectedPath As New System.Drawing.Drawing2D.GraphicsPath()
            'selectedPath.StartFigure()
            'selectedPath.AddArc(1, 1, frame.Height - 3, frame.Height - 3, 90, 180)
            'selectedPath.AddLine(frame.Height, 1, CInt(frame.Width - (frame.Height / 2)), 1) '- (frame.Height) - 1, 1)
            'selectedPath.AddArc(CInt(frame.Width - (frame.Height) + 1), 1, frame.Height - 3, frame.Height - 3, 270, 180)
            'selectedPath.AddLine(CInt(frame.Width - (frame.Height / 2)), frame.Height - 2, frame.Left + CInt(frame.Height / 2), frame.Height - 2)
            'selectedPath.CloseFigure()

            'Dim selectedPen As New Pen(Color.DimGray, 1)
            'selectedPen.DashStyle = Drawing2D.DashStyle.Dot

            'g.DrawPath(selectedPen, selectedPath)
            'End If


        End If

    End Sub

    Public Function PreferredWidth(Optional ByRef g As Graphics = Nothing) As Integer
        If TypeOf Me.ObjectValue Is TokenizableObject Then

            Dim gr As Graphics

            If g Is Nothing Then
                gr = Graphics.FromImage(New Bitmap(1, 1))
            Else
                gr = g
            End If
            Return gr.MeasureString(CType(Me.ObjectValue, TokenizableObject).TokenDisplayString, Me.Font).Width + 40
        End If
    End Function

    Private Function TokenRectForFrame(ByVal frame As Rectangle) As Rectangle
        Return New Rectangle(frame.X + 2, frame.Y + 2, frame.Width - 4, frame.Height - 4)
    End Function

    Public Overrides Function MouseDidMoveInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this method is whether the cell should be redrawn or not. 

        If TokenRectForFrame(relativeFrame).Contains(relativePoint) Then
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

        If TokenRectForFrame(relativeFrame).Contains(relativePoint) Then
            _isMouseOver = True
            '_preClickValue = MyBase.State
            ' MsgBox("Setting click status to " & _preClickValue)

            If TypeOf (Me.ObjectValue) Is TokenizableObject And relativePoint.X > relativeFrame.X + relativeFrame.Width - 20 Then
                Dim tokenObject As TokenizableObject = Me.ObjectValue

                If tokenObject.HasMenu Then
                    tokenObject.Menu.Show(Me.ParentControl.Control, New Point(_lastKnownDrawFrame.X + relativePoint.X, _lastKnownDrawFrame.Y + relativePoint.Y))
                    Return False
                End If

            End If

            Return True
        Else
            Return False
        End If

    End Function

    Public Overrides Function MouseDraggedInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this is whether the cell should be redrawn or not. 
        ' Note: This only gets called if the cell claimed ownership of the mouse operation in MouseDownInCell. 

        _isMouseOver = TokenRectForFrame(relativeFrame).Contains(relativePoint)

        'Dim data As DataObject = New DataObject
        'data.SetData(DataFormats.Serializable, False, Me.ObjectValue)

        If _isMouseOver Then
            If Me.AllowsCopy Then

                Me.ParentControl.Control.DoDragDrop(Me.ObjectValue, DragDropEffects.Copy)

            Else
                Me.ParentControl.Control.DoDragDrop(Me.ObjectValue, DragDropEffects.Move)

            End If
        End If

        Return True
    End Function

    Public Overrides Function MouseUpInCell(ByVal relativePoint As Point, ByVal relativeFrame As Rectangle) As Boolean
        ' The return of this method is whether the cell should be redrawn or not. 
        ' Note: This only gets called if the cell claimed ownership of the mouse operation in MouseDownInCell. 

        _isMouseDown = False

        If TokenRectForFrame(relativeFrame).Contains(relativePoint) Then
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


    Public Overrides Function NewInstance() As KNCell

        Dim cell As KNTokenCell = New KNTokenCell
        cell.Font = Me.Font
        cell.AllowsCopy = Me.AllowsCopy
        Return cell

    End Function


End Class
