Public Interface TokenizableObject

    Function TokenDisplayString() As String
    Function HasMenu() As Boolean
    Function Menu() As ContextMenuStrip
    Function Clone() As TokenizableObject

End Interface

