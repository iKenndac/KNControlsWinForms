
Imports System.Drawing.Drawing2D

Public Class KNLeopardCornerCell

    Inherits KNCell

    Private _drawSeparator As Boolean = False

    Public Overrides Sub DrawInFrame(ByRef g As Graphics, ByRef frame As Rectangle)

        If frame.Width = 0 Or frame.Height = 0 Then
            Return
        End If

        Dim startColor As Color = Color.FromArgb(191, 191, 191)
        Dim endColor As Color = Color.FromArgb(230, 230, 230)

        Dim gradient As LinearGradientBrush = New LinearGradientBrush(frame, endColor, startColor, LinearGradientMode.Vertical)

        g.FillRectangle(gradient, frame)

        g.DrawLine(Pens.DarkGray, frame.X, frame.Y, frame.X + frame.Width - 1, frame.Y)
        g.DrawLine(Pens.DarkGray, frame.X, frame.Y + frame.Height, frame.X + frame.Width - 1, frame.Y + frame.Height)

        If _drawSeparator Then
            g.DrawLine(Pens.DarkGray, frame.X + frame.Width - 1, frame.Y, frame.X + frame.Width - 1, frame.Y + frame.Height)
        End If



    End Sub


    Public Overrides Function NewInstance() As KNCell

        Return New KNLeopardCornerCell
    End Function


    Public Sub New(Optional ByVal drawSeparator As Boolean = False)
        _drawSeparator = drawSeparator
    End Sub



    Public Property DrawSeparator() As Boolean
        Get
            Return _drawSeparator
        End Get
        Set(ByVal value As Boolean)
            _drawSeparator = value
        End Set
    End Property


End Class

