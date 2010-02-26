Public Class KNHeaderCell
    Inherits KNActionCell

    Private _column As KNTableColumn

    Private triUp As Bitmap = New Bitmap(Me.GetType(), "TriangleUp.png")
    Private tiDown As Bitmap = New Bitmap(Me.GetType(), "TriangleDown.png")

    Public Overridable Shadows Sub DrawInFrame(ByRef g As Graphics, ByRef frame As Rectangle, ByVal _sortOrder As SortOrder)

        g.FillRectangle(Brushes.AliceBlue, frame)

    End Sub

    Public Overridable Sub DrawSortingIndiator(ByRef g As Graphics, ByVal frame As Rectangle, ByVal sortOrder As SortOrder)
        ' Override this to draw a custom indicator 


        If sortOrder <> Windows.Forms.SortOrder.None Then

            If sortOrder = Windows.Forms.SortOrder.Ascending Then

                g.DrawImage(triUp, SortingIndicatorRectForFrame(frame))

            Else

                g.DrawImage(tiDown, SortingIndicatorRectForFrame(frame))

            End If


        End If

    End Sub

    Public Overridable Function SortingIndicatorRectForFrame(ByVal frame As Rectangle) As Rectangle
        ' Override this to put the indicator in a different location (i.e., not the right hand side)

        ' For now, we assume it's 13x9! (bad idea)

        Dim margin As Integer = (frame.Height - 9) / 2

        Return New Rectangle(frame.X + frame.Width - margin - 13, margin, 13, 9)

    End Function


    Public Property Column() As KNTableColumn
        Get
            Return _column
        End Get
        Set(ByVal value As KNTableColumn)
            _column = value
        End Set
    End Property



End Class
