Imports KNControls

Public Class Form1

    Implements KNTableViewDataSource
    Implements KNTableViewDelegate


    Private _tokens() As KNSortingPresetToken = {New KNSortingPresetToken(KNSortingPresetToken.TokenType.kTokenTypeTitle, 0), _
                                                 New KNSortingPresetToken(KNSortingPresetToken.TokenType.kTokenTypeArtist, 0), _
                                                 New KNSortingPresetToken(KNSortingPresetToken.TokenType.kTokenTypeAlbum, 0), _
                                                 New KNSortingPresetToken(KNSortingPresetToken.TokenType.kTokenTypeRating, KNSortingPresetToken.RatingFormatMenuTags.kRatingFormatNumberStars), _
                                                 New KNSortingPresetToken(KNSortingPresetToken.TokenType.kTokenTypeTrack, 0)}


    Private _strings() As String = {"Value 1", "Value 2", "Value 3", "Value 4", "Value 5", "Value 6", "Value 7", "Value 8", "Value 9", "Value 10", _
                                     "Value 11", "Value 12", "Value 13", "Value 14", "Value 15", "Value 16", "Value 17", "Value 18", "Value 19", "Value 20"}


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Set the table's delegate and datasource.
        ' The table asks the delegate for behavioural information, 
        ' and the datasource for the source data. 

        KnTableView1.DataSource = Me
        KnTableView1.TableDelegate = Me

        EditableCellTable.DataSource = Me
        EditableCellTable.TableDelegate = Me

        ' Set up the table: Create a cell...
        Dim cell As KNTokenCell = New KNTokenCell
        cell.AllowsCopy = True

        ' Then a column, and set the column's identifier (name), datacell (the cell above, will be used to draw each row of the column),
        ' header cell (the cell to draw at the top of the column)
        KnTableView1.AddColumn(New KNTableColumn("index", cell, New KNLeopardStyleHeaderCell("A Table With Tokens In It", False)))
        ' The corner cell is the cell used to draw the parts of the table's header that doesn't contain a column
        KnTableView1.CornerCell = New KNLeopardCornerCell

        ' Setting up more cells and columns.
        ' A column contains a data cell and a header cell, and a table contains columns.
        Dim editableCell As KNTextCell = New KNTextCell
        editableCell.IsEditable = True


        EditableCellTable.AddColumn(New KNTableColumn("index", editableCell, New KNLeopardStyleHeaderCell("Editable Cells!")))
        Dim col As KNTableColumn = New KNTableColumn("blank", Nothing, New KNLeopardStyleiTunesHeaderCell)
        col.MaximumSize = 200
        col.MinimumSize = 75
        EditableCellTable.AddColumn(col)

        EditableCellTable.CornerCell = New KNLeopardCornerCell

        col = New KNTableColumn("blank", Nothing, New KNLeopardStyleHeaderCell(""))
        col.Width = 100
        col.MinimumSize = 100
        col.MaximumSize = 100
        EditableCellTable.AddColumn(col)




        Me.RatingChanged()

        ' Set the token field's initial tokens
        Dim items(3) As Object
        items(0) = "Prefix"
        items(1) = New KNSortingPresetToken(KNSortingPresetToken.TokenType.kTokenTypeTrack, 1)
        items(2) = " - "
        items(3) = New KNSortingPresetToken(KNSortingPresetToken.TokenType.kTokenTypeTitle, 0)

        KnTokenField1.Items = items



    End Sub

    Public Sub Changed() Handles KnTokenField1.ItemsDidChange

        ' When the token field's items change, update our label

        Dim str As String = ""

        For Each item As Object In KnTokenField1.Items
            str = str & "[" & item.ToString & "]"
        Next

        Label1.Text = str

    End Sub


    Public Sub Action(ByVal Row As Integer, ByRef column As KNTableColumn, ByVal Cell As KNActionCell) Handles EditableCellTable.CellPerformedAction

        ' This is fired when an editable cell does something. Get the new value from Cell.ObjectValue, and set it in your data model.
        ' The table will automatically reload after this is called, so this is your only chance to commit the edit.
        _strings(Row) = Cell.ObjectValue
    End Sub



    Public Function NumberOfItemsInTableView(ByRef tableView As KNControls.KNTableView) As Integer Implements KNControls.KNTableViewDataSource.NumberOfItemsInTableView

        ' DataSource callback: "How many rows do I have?"

        If tableView Is KnTableView1 Then
            Return _tokens.Length
        Else
            Return _strings.Length
        End If


    End Function

    Public Function ObjectForRow(ByRef tableView As KNControls.KNTableView, ByRef tableColumn As KNControls.KNTableColumn, ByVal rowIndex As Integer) As Object Implements KNControls.KNTableViewDataSource.ObjectForRow

        ' DataSource callback: "What object goes in row x of this column?"
        ' (The data cell in the given column needs to understand what to do with the returned object)

        If tableView Is KnTableView1 Then
            Return _tokens(rowIndex)
        Else
            Return _strings(rowIndex)
        End If

    End Function

    Public Function ColumnHeaderClicked(ByRef tableView As KNControls.KNTableView, ByRef column As KNControls.KNTableColumn, ByVal suggestedNewSortOrder As System.Windows.Forms.SortOrder) As System.Windows.Forms.SortOrder Implements KNControls.KNTableViewDelegate.ColumnHeaderClicked

        ' Delegate callback: Someone clicked a header, I think the column should be sorted like this. How should I sort?
        ' To use the default sort order, just return suggestedNewSortOrder

        Return suggestedNewSortOrder
    End Function

    Function ShouldSelectRow(ByRef tableView As KNTableView, ByVal rowIndex As Integer) As Boolean Implements KNTableViewDelegate.ShouldSelectRow

        'Delegate callback: Should I select this row?
        ' Sometimes, you don't want the user to be able to select a row.

        Return True
    End Function


    Private Sub RatingChanged() Handles KnRatingField1.RatingChanged

        StarsField.Text = (KnRatingField1.Rating / 20) & " stars"
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        ' Add myself as an observer for the notification called "somethingWickedHappened"
        ' Notifications are used when you're interested in something that you're not coupled to.
        ' For instance, you might care when a media player somewhere in your app stops playing. 
        ' If the media player supports notifications, just add yourself to the default notification
        ' centre for the media player's stopped notification. When the media player sends a notification
        ' that it stopped, you'll be notified.

        ' tl;dr: Be told about things without the sender or you being coupled to each other. 

        KNNotificationCentre.DefaultNotificationCentre. _
            AddObserverForNotificationName(New KNNotificationDelegate(AddressOf somethingHappened), "somethingWickedHappened")


    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        ' Remove myself as an observer for all notifications

        KNNotificationCentre.DefaultNotificationCentre.RemoveObserver(Me)

    End Sub



    Private Sub somethingHappened(ByRef notification As KNNotification)

        ' Called when a notification is fired.

        MsgBox(notification.Name)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        ' Post a test notification
        KNNotificationCentre.DefaultNotificationCentre.PostNotificationWithName("somethingWickedHappened", Button3)



    End Sub

    Private Sub KnLeopardStyleHeaderButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KnLeopardStyleHeaderButton1.click
        MsgBox("Clicked!")
    End Sub

    Private Sub LickToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LickToolStripMenuItem.Click

    End Sub
End Class


