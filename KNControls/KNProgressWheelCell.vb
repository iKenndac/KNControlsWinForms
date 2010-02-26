Imports System.Math
Imports System.Drawing.Drawing2D

Public Class KNProgressWheelCell
    Inherits KNCell


    Private WithEvents _wheelTimer As Timer = New Timer()

    Private _location As Integer
    Private _blobSize As Integer = 4


    Public Sub New()

        _wheelTimer.Interval = 150
        _wheelTimer.Start()
        _wheelTimer.Enabled = True

    End Sub

    Private Sub tickTocked(ByVal sender As Object, ByVal e As EventArgs) Handles _wheelTimer.Tick

        If _location = 7 Then
            _location = 0
        Else
            _location += 1
        End If

        If Not ParentControl Is Nothing Then
            ParentControl.UpdateCell(Me)
        End If

    End Sub

    Public Overrides Sub DrawInFrame(ByRef g As Graphics, ByRef frame As Rectangle)

        Dim circleRect As Rectangle
        Dim circleRadius As Integer
        Dim centrePoint As Point

        If frame.Width > frame.Height Then
            circleRect = New Rectangle(frame.X + (frame.Width - frame.Height) / 2, frame.Y, frame.Height, frame.Height)
        Else
            circleRect = New Rectangle(frame.X, frame.Y + (frame.Height - frame.Width) / 2, frame.Height, frame.Height)
        End If

        circleRadius = (circleRect.Width / 2) - _blobSize
        centrePoint = New Point(circleRect.X + (circleRect.Width / 2), circleRect.Y + (circleRect.Height / 2))

        ' Angles are 0, 45, 90, 135, 180, 225, 270 and 315 degrees

        Dim currentStep As Integer = 0
        Dim currentAngle As Integer

        Dim oldSmoothingMode As SmoothingMode = g.SmoothingMode
        g.SmoothingMode = SmoothingMode.HighQuality


        For currentStep = 0 To 7 Step 1
            currentAngle = 45 * currentStep
            Dim degreesRad = (currentAngle - 90) / 180 * PI
            Dim LineEndX As Integer = centrePoint.X + Cos(degreesRad) * circleRadius
            Dim LineEndY As Integer = centrePoint.Y + Sin(degreesRad) * circleRadius

            Dim blobRect As Rectangle = New Rectangle(LineEndX - (_blobSize / 2), LineEndY - (_blobSize / 2), _blobSize, _blobSize)

            Dim blobBrush As SolidBrush

            If currentStep = _location And Enabled Then
                blobBrush = New SolidBrush(Color.Gray)
            ElseIf (currentStep = _location - 1 Or (currentStep = 7 And _location = 0)) And Enabled Then
                blobBrush = New SolidBrush(Color.DarkGray)
            Else
                blobBrush = New SolidBrush(Color.LightGray)
            End If


            g.FillEllipse(blobBrush, blobRect)

        Next


        g.SmoothingMode = oldSmoothingMode

    End Sub

    Public Overrides Property Enabled() As Boolean
        Get
            Return MyBase.Enabled
        End Get
        Set(ByVal value As Boolean)
            _wheelTimer.Enabled = value
            If value = True Then
                _wheelTimer.Start()
            Else
                _wheelTimer.Stop()
            End If
            MyBase.Enabled = value
        End Set
    End Property


    Public Overrides Function NewInstance() As KNCell

        Return New KNProgressWheelCell

    End Function

End Class
