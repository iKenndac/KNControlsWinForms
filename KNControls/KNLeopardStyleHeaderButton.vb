Option Explicit On

Imports System.ComponentModel

Public Class KNLeopardStyleHeaderButton

    Private _style As ButtonStyleEnum = ButtonStyleEnum.Leopard
    Private _icon As System.Drawing.Image
    Private startColor As Color
    Private endColor As Color
    Private drawSelected As Boolean = False
    Private _borders As Integer
    Private _textAlignment As ContentAlignment = ContentAlignment.MiddleCenter
    Private _imageAlignment As ContentAlignment = ContentAlignment.MiddleLeft
    Private _image As Bitmap
    Private _text As String = ""


    Public Enum ButtonBorders As Integer
        BorderTop = 1
        BorderLeft = 2
        BorderRight = 4
        BorderBottom = 8
    End Enum

    Public Shadows Property Enabled() As Boolean
        Get
            Return MyBase.Enabled
        End Get
        Set(ByVal value As Boolean)
            MyBase.Enabled = value
            MyBase.Invalidate()
        End Set
    End Property

    Private Sub KeyIsDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Return Then

            RaiseEvent click(Me, New EventArgs())
        ElseIf e.KeyCode = Keys.Space Then
            drawSelected = True
            MyBase.Invalidate()
        End If
    End Sub

    Private Sub KeyIsUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyUp
        If Val(e.KeyCode) = Keys.Space Then
            drawSelected = False
            MyBase.Invalidate()
            RaiseEvent click(Me, New EventArgs())
        End If
    End Sub

    Private Sub DidReceiveFocus(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.GotFocus
        MyBase.Invalidate()
    End Sub

    Private Sub DidLoseFocus(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.LostFocus
        MyBase.Invalidate()
    End Sub

    Public Shadows Event click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        'MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics

        Dim drawStartColor As Color
        Dim drawEndColor As Color
        Select Case _style
            Case ButtonStyleEnum.Leopard
                drawStartColor = Color.FromArgb(191, 191, 191)
                drawEndColor = Color.FromArgb(230, 230, 230)

            Case ButtonStyleEnum.Tiger
                drawStartColor = Color.FromArgb(211, 211, 211)
                drawEndColor = Color.FromArgb(250, 250, 250)
            Case Else
                'use custom color
                drawStartColor = startColor
                drawEndColor = endColor
        End Select

        If drawSelected Then
            Dim sR, sG, sB, eR, eG, eB As Integer

            sR = IIf(drawStartColor.R > 40, drawStartColor.R - 40, 0)
            sG = IIf(drawStartColor.G > 40, drawStartColor.G - 40, 0)
            sB = IIf(drawStartColor.B > 40, drawStartColor.B - 40, 0)

            eR = IIf(drawEndColor.R > 40, drawEndColor.R - 40, 0)
            eG = IIf(drawEndColor.G > 40, drawEndColor.G - 40, 0)
            eB = IIf(drawEndColor.B > 40, drawEndColor.B - 40, 0)

            drawStartColor = Color.FromArgb(sR, sG, sB)
            drawEndColor = Color.FromArgb(eR, eG, eB)
        End If

        Dim gradientBrush As New System.Drawing.Drawing2D.LinearGradientBrush _
        (New Point(0, 0), New Point(0, Height), drawEndColor, drawStartColor)

        g.FillRectangle(gradientBrush, ClientRectangle)


        ' Borders

        If (Borders And ButtonBorders.BorderBottom) Then
            g.DrawLine(Pens.DarkGray, 0, Height - 1, Width, Height - 1)
        End If

        If (Borders And ButtonBorders.BorderTop) Then
            g.DrawLine(Pens.DarkGray, 0, 0, Width, 0)
        End If

        If (Borders And ButtonBorders.BorderLeft) Then
            g.DrawLine(Pens.DarkGray, 0, 0, 0, Height)
        End If

        If (Borders And ButtonBorders.BorderRight) Then
            g.DrawLine(Pens.DarkGray, Width - 1, 0, Width - 1, Height)
        End If







        gradientBrush.Dispose()

        'Button background and borders drawn
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Declare brushes for drawing text

        Dim foreBrush As SolidBrush

        If Enabled Then
            foreBrush = New SolidBrush(ForeColor)
        Else
            Dim x As Color = Me.ForeColor

            Dim red As Integer = x.R + 126
            Dim green As Integer = x.G + 126
            Dim blue As Integer = x.B + 126

            If red > 255 Then
                red = 255
            End If

            If green > 255 Then
                green = 255
            End If

            If blue > 255 Then
                blue = 255
            End If


            foreBrush = New SolidBrush(Color.FromArgb(red, green, blue))
        End If

        Dim shaddowBrush As New SolidBrush(Color.FromArgb(100, 255, 255, 255))
        Dim strFormat As New StringFormat

        Select Case TextAlign
            Case ContentAlignment.TopCenter, ContentAlignment.TopLeft, ContentAlignment.TopRight
                strFormat.LineAlignment = StringAlignment.Near 'Vertical alignment
            Case ContentAlignment.MiddleCenter, ContentAlignment.MiddleRight, ContentAlignment.MiddleLeft
                strFormat.LineAlignment = StringAlignment.Center 'Vertical alignment
            Case ContentAlignment.BottomCenter, ContentAlignment.BottomLeft, ContentAlignment.BottomRight
                strFormat.LineAlignment = StringAlignment.Far 'Vertical alignment
        End Select

        Select Case TextAlign
            Case ContentAlignment.BottomCenter, ContentAlignment.MiddleCenter, ContentAlignment.TopCenter
                strFormat.Alignment = StringAlignment.Center
            Case ContentAlignment.BottomLeft, ContentAlignment.MiddleLeft, ContentAlignment.TopLeft
                strFormat.Alignment = StringAlignment.Near
            Case ContentAlignment.BottomRight, ContentAlignment.MiddleRight, ContentAlignment.TopRight
                strFormat.Alignment = StringAlignment.Far
        End Select

        Dim _imageY As Integer

        If Image IsNot Nothing Then
            Select Case ImageAlign
                Case ContentAlignment.BottomCenter, ContentAlignment.BottomLeft, ContentAlignment.BottomRight
                    _imageY = Height - (Image.Height + 1)
                Case ContentAlignment.MiddleCenter, ContentAlignment.MiddleLeft, ContentAlignment.MiddleRight
                    _imageY = (Height / 2) - Image.Height / 2
                Case ContentAlignment.TopCenter, ContentAlignment.TopLeft, ContentAlignment.TopRight
                    _imageY = 1
            End Select
        End If

        Dim margin As Integer = 4
        Dim attribs As New Imaging.ImageAttributes
        Dim matrix As New Imaging.ColorMatrix
        matrix.Matrix00 = 1.0
        matrix.Matrix11 = 1.0
        matrix.Matrix22 = 1.0
        If Me.Enabled Then
            matrix.Matrix33 = 1.0
        Else
            matrix.Matrix33 = 0.5 'Opancy, 0.5F is 50%
        End If
        matrix.Matrix44 = 1.0
        attribs.SetColorMatrix(matrix)


        Select Case ImageAlign
            Case ContentAlignment.MiddleLeft, ContentAlignment.TopLeft, ContentAlignment.BottomLeft
                If Image Is Nothing Then
                    'draw text - no offset
                    g.DrawString(Text, Font, shaddowBrush, New RectangleF(margin + 1, 2, Width - margin, Height - 2), strFormat)
                    g.DrawString(Text, Font, foreBrush, New RectangleF(margin, 1, Width - margin, Height - 2), strFormat)
                Else
                    g.DrawImage(Image, New Rectangle(margin, _imageY, Image.Width, Image.Height), 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, attribs)
                    g.DrawString(Text, Font, shaddowBrush, New RectangleF(margin + 1 + Image.Width + margin, 2, Width - ((Image.Width) + 3), Height - 2), strFormat)
                    g.DrawString(Text, Font, foreBrush, New RectangleF(margin + Image.Width + margin, 1, Width - margin - ((Image.Width) + 3), Height - 2), strFormat)
                End If
            Case ContentAlignment.MiddleRight, ContentAlignment.TopRight, ContentAlignment.BottomRight
                If Image Is Nothing Then
                    'draw text - no offset
                    g.DrawString(Text, Font, shaddowBrush, New RectangleF(2, 2, Width - 2, Height - 2), strFormat)
                    g.DrawString(Text, Font, foreBrush, New RectangleF(1, 1, Width - 2, Height - 2), strFormat)
                Else
                    g.DrawImage(Image, New Rectangle(Width - (Image.Width + 2), _imageY, Image.Width, Image.Height), 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, attribs)
                    g.DrawString(Text, Font, shaddowBrush, New RectangleF(2 + Image.Width, 2, Width - ((Image.Width * 2) + 3), Height - 2), strFormat)
                    g.DrawString(Text, Font, foreBrush, New RectangleF(1 + Image.Width, 1, Width - ((Image.Width * 2) + 3), Height - 2), strFormat)
                End If
            Case ContentAlignment.MiddleCenter, ContentAlignment.TopCenter, ContentAlignment.BottomCenter
                If Image Is Nothing Then
                    'draw text instead
                    g.DrawString(Text, Font, shaddowBrush, New RectangleF(2, 2, Width - 4, Height - 2), strFormat)
                    g.DrawString(Text, Font, foreBrush, New RectangleF(1, 1, Width - 2, Height - 2), strFormat)
                Else
                    g.DrawImage(Image, New Rectangle((Width / 2) - Image.Width / 2, _imageY, Image.Width, Image.Height), 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, attribs)
                    'g.DrawImage(Image, New PointF((Width / 2) - Image.Width / 2, _imageY))
                End If
        End Select


        If MyBase.ContainsFocus Then

            Dim focusRect As Rectangle = New Rectangle(3, 3, MyBase.Width - 7, MyBase.Height - 7)
            Dim pen As Pen = New Pen(Color.Black, 1)
            pen.DashStyle = Drawing2D.DashStyle.Dot

            g.DrawRectangle(pen, focusRect)

        End If

        shaddowBrush.Dispose()
        foreBrush.Dispose()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal mevent As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(mevent)
        drawSelected = True
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal mevent As System.Windows.Forms.MouseEventArgs)
        'OnMouseUp(mevent)
        drawSelected = False
        Invalidate()
        If mevent.X >= 0 And mevent.X < Width And mevent.Y >= 0 And mevent.Y < Height Then
            RaiseEvent click(Me, Nothing)
        End If
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal mevent As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(mevent)

        Select Case mevent.Button
            Case Windows.Forms.MouseButtons.Left
                If mevent.X >= 0 And mevent.X < Width And mevent.Y >= 0 And mevent.Y < Height Then
                    'inside control bounds
                    drawSelected = True
                Else
                    drawSelected = False
                End If
        End Select
    End Sub

    Private Sub resized(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Resize
        MyBase.Invalidate()
    End Sub

    Public Property Style() As ButtonStyleEnum
        Get
            Return _style
        End Get
        Set(ByVal value As ButtonStyleEnum)
            _style = value
            MyBase.Invalidate()
        End Set
    End Property
    Public Property CustomStartColor() As Color
        Get
            Return startColor
        End Get
        Set(ByVal value As Color)
            startColor = value
        End Set
    End Property
    Public Property CustomEndColor() As Color
        Get
            Return endColor
        End Get
        Set(ByVal value As Color)
            endColor = value
        End Set
    End Property


    Public Property Borders() As Integer
        Get
            Return _borders
        End Get
        Set(ByVal value As Integer)
            _borders = value
            MyBase.Invalidate()
        End Set
    End Property

    Public Property ImageAlign() As ContentAlignment
        Get
            Return _imageAlignment
        End Get
        Set(ByVal value As ContentAlignment)
            _imageAlignment = value
            MyBase.Invalidate()
        End Set
    End Property

    Public Property TextAlign() As ContentAlignment
        Get
            Return _textAlignment
        End Get
        Set(ByVal value As ContentAlignment)
            _textAlignment = value
            MyBase.Invalidate()
        End Set
    End Property

    Public Property Image() As Bitmap
        Get
            Return _image
        End Get
        Set(ByVal value As Bitmap)
            _image = value
            MyBase.Invalidate()
        End Set
    End Property

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), BrowsableAttribute(True)> _
    Public Overrides Property Text() As String
        Get
            Return _text
        End Get
        Set(ByVal value As String)
            _text = value
            MyBase.Invalidate()
        End Set
    End Property

    Public Enum ButtonStyleEnum
        Leopard = 0
        Tiger = 1
        Custom = 99
    End Enum


End Class
