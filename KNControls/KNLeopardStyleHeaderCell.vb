Imports System.Drawing.Drawing2D

Public Class KNLeopardStyleHeaderCell
    Inherits KNHeaderCell


    Private _title As String
    Private _drawSeparator As Boolean
    Private _textFont As Font = New Font("Tahoma", 9, FontStyle.Bold)
    Private _allowsClick As Boolean
    Private _alignment As StringAlignment = StringAlignment.Near


    Public Sub New(Optional ByVal title As String = "", Optional ByVal drawsSeparator As Boolean = True, Optional ByVal allowsClick As Boolean = True, Optional ByVal alignment As StringAlignment = StringAlignment.Near)
        _title = title
        _drawSeparator = drawsSeparator
        _allowsClick = allowsClick
        _alignment = alignment
    End Sub

    Public Overrides Sub DrawInFrame(ByRef g As Graphics, ByRef frame As Rectangle, ByVal _sortOrder As SortOrder)

        Dim startColor As Color
        Dim endColor As Color

        If frame.Width = 0 Or frame.Height = 0 Then
            Return
            ' No need to bother
        End If

        If State = KNCellState.KNOffState Or Not _allowsClick Then

            If (Column.SortingPriority = KNTableColumn.SortPriority.Primary) Then
                startColor = Color.FromArgb(173, 173, 173)
                endColor = Color.FromArgb(212, 212, 212)

            Else
                startColor = Color.FromArgb(191, 191, 191)
                endColor = Color.FromArgb(230, 230, 230)
            End If

        ElseIf State = KNCellState.KNOnState Then
            startColor = Color.FromArgb(153, 153, 153)
            endColor = Color.FromArgb(191, 191, 191)

        End If

        Dim gradient As LinearGradientBrush = New LinearGradientBrush(frame, endColor, startColor, LinearGradientMode.Vertical)

        g.FillRectangle(gradient, frame)

        g.DrawLine(Pens.DarkGray, frame.X, frame.Y, frame.X + frame.Width - 1, frame.Y)
        g.DrawLine(Pens.DarkGray, frame.X, frame.Y + frame.Height, frame.X + frame.Width - 1, frame.Y + frame.Height)

        If _drawSeparator Then
            g.DrawLine(Pens.DarkGray, frame.X + frame.Width - 1, frame.Y, frame.X + frame.Width - 1, frame.Y + frame.Height)
        End If


        Dim height As Integer = g.MeasureString(_title, _textFont, New SizeF(frame.Width - 10, frame.Height - 6)).Height

        Dim textRect As Rectangle = New Rectangle(frame.X + 5, (frame.Height / 2) - (height / 2), frame.Width - 10 - 15, frame.Height - 2)
        Dim whiteTextRect As Rectangle = New Rectangle(textRect.X + 1, textRect.Y + 1, textRect.Width, textRect.Height)

        Dim format As StringFormat = New StringFormat()
        format.Alignment = _alignment
        format.Trimming = StringTrimming.EllipsisCharacter

        g.DrawString(_title, _textFont, New SolidBrush(Color.FromArgb(100, 255, 255, 255)), whiteTextRect, format)
        g.DrawString(_title, _textFont, Brushes.Black, textRect, format)

        If Not Column Is Nothing Then
            If (Column.SortingPriority = KNTableColumn.SortPriority.Primary) Then
                DrawSortingIndiator(g, frame, _sortOrder)
            End If
        End If



    End Sub

    Public Overrides Function NewInstance() As KNCell

        Return New KNLeopardStyleHeaderCell

    End Function

    Public Property TextFont() As Font
        Get
            Return _textFont
        End Get
        Set(ByVal value As Font)
            _textFont = value
        End Set
    End Property

End Class
