Imports System.Runtime.Serialization

Public Class KNSortingPresetToken
    Implements TokenizableObject
    Implements ISerializable

#Region "Enums"

    Public Enum TokenType
        kTokenTypeTitle = 1
        kTokenTypeArtist = 2
        kTokenTypeAlbum = 3
        kTokenTypeGenre = 4
        kTokenTypeLength = 5
        kTokenTypeTrack = 6
        kTokenTypeTrackOf = 7
        kTokenTypeRating = 8
        kTokenTypePlaycount = 9
        kTokenTypeComposer = 10
        kTokenTypeYear = 11
        kTokenTypeAlbumArtist = 12
        kTokenTypeDisc = 13
        kTokenTypeDiscOf = 14

        kTokenTypeDirectory = 20
        kTokenTypeExtension = 21
        kTokenTypeOriginalFileName = 22
        kTokenTypeFilesize = 23
        kTokenTypeFileType = 24
    End Enum

    Public Enum SigDigitsMenuTags
        kOneSigDigits = 0
        kTwoSigDigits = 1
        kThreeSigDigits = 2
    End Enum

    Public Enum ExtensionFormatMenuTags
        kExtensionFormatWithDot = 0
        kExtensionFormatWithoutDot = 1
    End Enum

    Public Enum RatingFormatMenuTags
        kRatingFormatAsterisks = 0
        kRatingFormatNumberStars = 1
        kRatingFormatWordStars = 2
    End Enum


#End Region

    Private _tokenType As TokenType = TokenType.kTokenTypeTitle
    Private _menuTag = 0


#Region "TokenizableObject Delegates"

    Public Function HasMenu() As Boolean Implements TokenizableObject.HasMenu
        Select Case Me.Type
            Case TokenType.kTokenTypeDisc, TokenType.kTokenTypeDiscOf, TokenType.kTokenTypeTrack, TokenType.kTokenTypeTrackOf
                Return True
            Case TokenType.kTokenTypeRating
                Return True
            Case TokenType.kTokenTypeExtension
                Return True
            Case Else
                Return False
        End Select

    End Function

    Public Function Menu() As System.Windows.Forms.ContextMenuStrip Implements TokenizableObject.Menu
        Return GenerateMenu()
    End Function

    Public Function TokenDisplayString() As String Implements TokenizableObject.TokenDisplayString
        Select Case Me.Type
            Case TokenType.kTokenTypeDisc
                Return "Disc"

            Case (TokenType.kTokenTypeDiscOf)
                Return "Disc Of"

            Case TokenType.kTokenTypeArtist
                Return "Artist"
            Case TokenType.kTokenTypeAlbum
                Return "Album"
            Case TokenType.kTokenTypeAlbumArtist
                Return "Album Artist"
            Case TokenType.kTokenTypeComposer
                Return "Composer"
            Case TokenType.kTokenTypeExtension

                Select Case _menuTag
                    Case ExtensionFormatMenuTags.kExtensionFormatWithDot
                        Return ".Extension"
                    Case Else
                        Return "Extension"
                End Select

            Case TokenType.kTokenTypeFilesize
                Return "Filesize"
            Case TokenType.kTokenTypeFileType
                Return "File Type"
            Case TokenType.kTokenTypeGenre
                Return "Genre"
            Case TokenType.kTokenTypeLength
                Return "Track Length"
            Case TokenType.kTokenTypeOriginalFileName
                Return "Orig. Filename"
            Case TokenType.kTokenTypePlaycount
                Return "Play Count"
            Case TokenType.kTokenTypeRating

                Select Case _menuTag
                    Case RatingFormatMenuTags.kRatingFormatNumberStars
                        Return "Rating (4 Stars)"
                    Case RatingFormatMenuTags.kRatingFormatWordStars
                        Return "Rating (Four Stars)"
                    Case RatingFormatMenuTags.kRatingFormatAsterisks
                        Return "****"
                    Case Else
                        Return "Rating"
                End Select

            Case TokenType.kTokenTypeTitle
                Return "Title"
            Case TokenType.kTokenTypeTrack
                Return "Track"

            Case TokenType.kTokenTypeTrackOf
                Return "Track Of"

            Case TokenType.kTokenTypeYear
                Return "Year"
            Case Else
                Return "----"
        End Select
    End Function

    Public Function Clone() As TokenizableObject Implements TokenizableObject.Clone
        Return New KNSortingPresetToken(Me.ValuesDictionary)
    End Function

#End Region

#Region "Properties"

    Public Property Type() As TokenType
        Get
            Return _tokenType
        End Get
        Set(ByVal value As TokenType)
            _tokenType = value
        End Set
    End Property

    Public Property TypeModifier() As Integer
        Get
            Return _menuTag
        End Get
        Set(ByVal value As Integer)
            _menuTag = value
        End Set
    End Property
#End Region

    Public Sub New()
        _tokenType = TokenType.kTokenTypeTitle
        _menuTag = 0
    End Sub

    Public Sub New(ByVal Type As TokenType, ByVal TypeModifier As Integer)
        Me.Type = Type
        Me.TypeModifier = TypeModifier
    End Sub

    Public Sub New(ByVal dict As Dictionary(Of String, Object))

        If (dict.ContainsKey("value")) Then
            _tokenType = CType(dict.Item("value"), TokenType)
        End If
        If (dict.ContainsKey("menuindex")) Then
            _menuTag = CType(dict.Item("menuindex"), Integer)
        End If

    End Sub

    Private Sub New(ByVal Info As SerializationInfo, ByVal Context As StreamingContext)

        With Info
            Me.Type = .GetInt32("type")
            Me.TypeModifier = .GetInt32("typeModifier")
        End With
    End Sub

    Public Function ValuesDictionary() As Dictionary(Of String, Object)
        Dim dict As New Dictionary(Of String, Object)
        dict.Add("value", _tokenType)
        dict.Add("menuindex", _menuTag)
        Return dict
    End Function

    Public Overrides Function ToString() As String
        Return TokenDisplayString()
    End Function


#Region "Menus"

    Private Function GenerateMenu() As ContextMenuStrip

        Select Case Me.Type
            Case TokenType.kTokenTypeDisc, TokenType.kTokenTypeDiscOf, TokenType.kTokenTypeTrack, TokenType.kTokenTypeTrackOf

                Dim menu As New ContextMenuStrip()
                menu.ShowCheckMargin = True
                menu.ShowImageMargin = False
                menu.RenderMode = ToolStripRenderMode.System

                Dim item As ToolStripMenuItem

                item = New ToolStripMenuItem("1")
                item.Tag = SigDigitsMenuTags.kOneSigDigits
                item.Checked = (CInt(_menuTag).Equals(CInt(SigDigitsMenuTags.kOneSigDigits)))
                AddHandler item.Click, AddressOf MenuItemWasClicked
                menu.Items.Add(item)

                item = New ToolStripMenuItem("01")
                item.Tag = SigDigitsMenuTags.kTwoSigDigits
                item.Checked = (CInt(_menuTag).Equals(CInt(SigDigitsMenuTags.kTwoSigDigits)))
                AddHandler item.Click, AddressOf MenuItemWasClicked
                menu.Items.Add(item)

                item = New ToolStripMenuItem("001")
                item.Tag = SigDigitsMenuTags.kThreeSigDigits
                item.Checked = (CInt(_menuTag).Equals(CInt(SigDigitsMenuTags.kThreeSigDigits)))
                AddHandler item.Click, AddressOf MenuItemWasClicked
                menu.Items.Add(item)


                Return menu

            Case TokenType.kTokenTypeRating


                Dim menu As New ContextMenuStrip()
                menu.ShowCheckMargin = True
                menu.ShowImageMargin = False
                menu.RenderMode = ToolStripRenderMode.System


                Dim item As ToolStripMenuItem


                item = New ToolStripMenuItem("4 Stars")
                item.Tag = RatingFormatMenuTags.kRatingFormatNumberStars
                item.Checked = (CInt(_menuTag).Equals(CInt(RatingFormatMenuTags.kRatingFormatNumberStars)))
                AddHandler item.Click, AddressOf MenuItemWasClicked
                menu.Items.Add(item)

                item = New ToolStripMenuItem("Four Stars")
                item.Tag = RatingFormatMenuTags.kRatingFormatWordStars
                item.Checked = (CInt(_menuTag).Equals(CInt(RatingFormatMenuTags.kRatingFormatWordStars)))
                AddHandler item.Click, AddressOf MenuItemWasClicked
                menu.Items.Add(item)

                Return menu

            Case TokenType.kTokenTypeExtension

                Dim menu As New ContextMenuStrip()
                menu.ShowCheckMargin = True
                menu.ShowImageMargin = False
                menu.RenderMode = ToolStripRenderMode.System

                Dim item As ToolStripMenuItem

                item = New ToolStripMenuItem("Extension")
                item.Tag = ExtensionFormatMenuTags.kExtensionFormatWithoutDot
                item.Checked = (CInt(_menuTag).Equals(CInt(ExtensionFormatMenuTags.kExtensionFormatWithoutDot)))
                AddHandler item.Click, AddressOf MenuItemWasClicked
                menu.Items.Add(item)

                item = New ToolStripMenuItem(".Extension")
                item.Tag = ExtensionFormatMenuTags.kExtensionFormatWithDot
                item.Checked = (CInt(_menuTag).Equals(CInt(ExtensionFormatMenuTags.kExtensionFormatWithDot)))
                AddHandler item.Click, AddressOf MenuItemWasClicked
                menu.Items.Add(item)

                Return menu

            Case Else
                Return Nothing

        End Select


    End Function

    Private Sub MenuItemWasClicked(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.TypeModifier = CInt(sender.Tag)
    End Sub

#End Region


    Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData

        With info
            .FullTypeName = _
                "ObjectSerialization.SortingPresetToken"
            .AddValue("type", _tokenType)
            .AddValue("typeModifier", _menuTag)
        End With

    End Sub
End Class
