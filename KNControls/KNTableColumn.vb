Public Class KNTableColumn
    Implements KNActionCellDelegate


    Private _cell As KNCell = New KNTextCell
    Private _headerCell As KNHeaderCell = New KNHeaderCell
    Private _width As Integer = 100
    Private _identifier As String

    Private _userResizable As Boolean = True ' Whether the user can resize the column 
    Private _minSize As Integer = 20 ' The minimum size of the column, respected in user and auto sizing
    Private _maxSize As Integer = Integer.MaxValue ' The maximum size of the column, respected in user and auto sizing
    Private _sortPriority As SortPriority = SortPriority.NotUsed

    Private _delegate As KNTableColumnDelegate

    Private _cells As Dictionary(Of Integer, KNCell) = New Dictionary(Of Integer, KNCell)

    Public Enum SortPriority
        Primary = 0
        Seconary = 1
        NotUsed = 2
    End Enum


    Public Sub New(Optional ByVal Identifier As String = "", Optional ByRef dataCell As KNCell = Nothing, Optional ByRef headerCell As KNHeaderCell = Nothing, _
             Optional ByRef del As KNTableColumnDelegate = Nothing)
        _identifier = Identifier

        If Not dataCell Is Nothing Then
            Me.DataCell = dataCell
        End If

        If Not headerCell Is Nothing Then
            headerCell.Column = Me
            _headerCell = headerCell
        End If

        _delegate = del


    End Sub


    Public Function CellForRow(ByVal Row As Integer) As KNCell


        If Not _cells.ContainsKey(Row) Then

            Dim newCell As KNCell = DataCell.NewInstance

            If TypeOf (newCell) Is KNActionCell Then
                CType(newCell, KNActionCell).CellDelegate = Me
            End If

            _cells.Item(Row) = newCell

        End If

        Return _cells.Item(Row)

    End Function

    Public Function RowForCell(ByRef cell As KNCell) As Integer
        If Not _cells.ContainsValue(cell) Then
            Return -1
        Else

            For Each key As Integer In _cells.Keys
                If ReferenceEquals(_cells.Item(key), cell) Then
                    Return key
                End If
            Next

        End If
    End Function


    Public Property Width() As Integer
        Get
            Return _width
        End Get
        Set(ByVal value As Integer)
            _width = value
        End Set
    End Property


    Public Property DataCell() As KNCell
        Get
            Return _cell
        End Get
        Set(ByVal value As KNCell)
            _cell = value
            _cells = New Dictionary(Of Integer, KNCell)
            If TypeOf (_cell) Is KNActionCell Then
                CType(_cell, KNActionCell).CellDelegate = Me
            End If
        End Set
    End Property

    Public Property Identifier() As String
        Get
            Return _identifier
        End Get
        Set(ByVal value As String)
            _identifier = value
        End Set
    End Property

    Public Property HeaderCell() As KNHeaderCell
        Get
            Return _headerCell
        End Get
        Set(ByVal value As KNHeaderCell)
            value.Column = Nothing
            _headerCell = value
            _headerCell.Column = Me
        End Set
    End Property

    Public Property UserResizable() As Boolean
        Get
            Return _userResizable
        End Get
        Set(ByVal value As Boolean)
            _userResizable = value
        End Set
    End Property

    Public Property MinimumSize() As Integer
        Get
            Return _minSize
        End Get
        Set(ByVal value As Integer)
            _minSize = value
        End Set
    End Property

    Public Property MaximumSize() As Integer
        Get
            Return _maxSize
        End Get
        Set(ByVal value As Integer)
            _maxSize = value
        End Set
    End Property

    Public Property SortingPriority() As SortPriority
        Get
            Return _sortPriority
        End Get
        Set(ByVal value As SortPriority)
            _sortPriority = value
        End Set
    End Property

    Public Property ColumnDelegate() As KNTableColumnDelegate
        Get
            Return _delegate
        End Get
        Set(ByVal value As KNTableColumnDelegate)
            _delegate = value
        End Set
    End Property

    Public Sub CellPerformedAction(ByRef Cell As KNActionCell) Implements KNActionCellDelegate.CellPerformedAction

        If Not _delegate Is Nothing Then
            _delegate.ActionCellPerformedAction(Cell, Me)
        End If
    End Sub

End Class

Public Interface KNTableColumnDelegate
    Sub ActionCellPerformedAction(ByRef Cell As KNActionCell, ByRef Column As KNTableColumn)
End Interface
