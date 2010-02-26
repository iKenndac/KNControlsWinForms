Public Class KNLeopardStyleiTunesHeaderCell
    Inherits KNLeopardStyleHeaderCell

    Private iTunesIcon As Bitmap = New Bitmap(Me.GetType(), "HeaderiTunes.png") ' 12x12


    Public Sub New(Optional ByVal drawsSeparator As Boolean = True, Optional ByVal allowsClick As Boolean = True)
        MyBase.New("", drawsSeparator, allowsClick, StringAlignment.Near)
    End Sub


    Public Overrides Function NewInstance() As KNCell
        Return New KNLeopardStyleiTunesHeaderCell
    End Function

    Public Overrides Sub DrawInFrame(ByRef g As Graphics, ByRef frame As Rectangle, ByVal _sortOrder As SortOrder)

        MyBase.DrawInFrame(g, frame, _sortOrder)

        Dim size As Size = New Size(12, 12)

        Dim sizeRect As Rectangle = New Rectangle(frame.X + ((frame.Width / 2) - (size.Width / 2)), frame.Y + 1 + ((frame.Height / 2) - (size.Height / 2)), _
                                                        size.Width, size.Height)

        Dim overlap As Integer = (frame.X + frame.Width) - ((sizeRect.X + sizeRect.Width) + 18)

        'Debug.Print(Format(overlap))


        If overlap < 0 Then
            sizeRect.X = sizeRect.X + overlap
        End If


        g.DrawImage(iTunesIcon, sizeRect)



    End Sub

End Class
