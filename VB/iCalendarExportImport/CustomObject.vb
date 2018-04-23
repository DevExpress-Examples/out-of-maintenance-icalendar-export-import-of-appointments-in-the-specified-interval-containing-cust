Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Drawing
Imports System.IO

Namespace iCalendarExportImport
    <Serializable> _
    Public Class CustomObject

        Private name_Renamed As String

        Private info_Renamed As String

        Private picture_Renamed() As Byte

        Public Sub New()
        End Sub
        Public Property Name() As String
            Get
                Return name_Renamed
            End Get
            Set(ByVal value As String)
                name_Renamed = value
            End Set
        End Property
        Public Property Info() As String
            Get
                Return info_Renamed
            End Get
            Set(ByVal value As String)
                info_Renamed = value
            End Set
        End Property
        Public Property PictureBytes() As Byte()
            Get
                Return picture_Renamed
            End Get
            Set(ByVal value As Byte())
                picture_Renamed = value
            End Set
        End Property
        Public Property Picture() As Image
            Get
                Dim ms As New MemoryStream(picture_Renamed)
                Return Image.FromStream(ms)

            End Get
            Set(ByVal value As Image)
                Dim ms As New MemoryStream()
                value.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp)
                picture_Renamed = ms.ToArray()
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("NAME={0} INFO={1}", Name, Info)
        End Function
    End Class

End Namespace
