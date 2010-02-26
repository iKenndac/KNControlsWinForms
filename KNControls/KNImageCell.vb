Public Class KNImageCell
    Inherits KNCell


    Public Overrides Sub DrawInFrame(ByRef g As Graphics, ByRef frame As Rectangle)

        If Not (ObjectValue Is Nothing) Then
            If TypeOf (ObjectValue) Is Image Then

                Dim img As Image = CType(ObjectValue, Image)

                Dim drawRect As Rectangle = New Rectangle(New Point(frame.X + (frame.Width / 2) - (img.Width / 2), frame.Y + (frame.Height / 2) - (img.Height / 2)), _
                                                    img.Size)

                g.DrawImage(ObjectValue, drawRect)


            End If


        End If



    End Sub

    Public Overrides Function NewInstance() As KNCell

        Return New KNImageCell

    End Function


End Class
