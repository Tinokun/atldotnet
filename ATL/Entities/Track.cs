using ATL.AudioData;
using ATL.AudioReaders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ATL
{
	/// <summary>
	/// Track description
	/// </summary>
	public class Track
	{
		public Track() {}

        public Track(String iPath, bool useOldImplementation = false)
        {
            Path = iPath;

            if (useOldImplementation) UpdateOld(); else Update();
        }

		public String Path;		
		public String Title;
		public String Artist;
        public String Composer;
		public String Comment;
		public String Genre;		
		public String Album;
		public int Year;
		public int Bitrate;
		public bool IsVBR;
		public int CodecFamily;
		public long Size;
		public long LastModified;
		public int Duration;		
		public int TrackNumber;
        public int DiscNumber;
        public int Rating;
        [Obsolete]
        public IList<MetaReaderFactory.PIC_CODE> Pictures;
        public IList<MetaDataIOFactory.PIC_TYPE> PictureTokens;

        protected Image coverArt = null;


        public Image GetEmbeddedPicture()
        {
            if (null == coverArt)
            {
                Update(new MetaDataIOFactory.PictureStreamHandlerDelegate(this.readImageData));
            }
            
            return coverArt;
        }

        // Kept for compatibility issues during parallel development
        protected void readImageData(ref MemoryStream s)
        {
            readImageData(ref s, MetaDataIOFactory.PIC_TYPE.Front);
        }

        protected void readImageData(ref MemoryStream s, MetaDataIOFactory.PIC_TYPE picCode)
        {
            coverArt = Image.FromStream(s);
        }

        [Obsolete]
        protected void UpdateOld(StreamUtils.StreamHandlerDelegate pictureStreamHandler = null)
        {
            FileInfo theFileInfo = new FileInfo(Path);

            if (theFileInfo.Exists)
            {
                Size = theFileInfo.Length;
                LastModified = theFileInfo.LastWriteTime.Ticks;
            }

            //TODO when tag is not available, customize by naming options // tracks (...)
            AudioFileReader theReader = new AudioFileReader(Path, pictureStreamHandler);

            // Per convention, the presence of a pictureStreamHandler
            // indicates that we only want coverArt updated
            if (null == pictureStreamHandler)
            {
                Title = theReader.Title;
                if ("" == Title || null == Title)
                {
                    Title = System.IO.Path.GetFileNameWithoutExtension(Path);
                }
                Artist = theReader.Artist;
                if (null == Artist) { Artist = ""; }
                Composer = theReader.Composer;
                if (null == Composer) { Composer = ""; }
                Comment = theReader.Comment;
                if (null == Comment) { Comment = ""; }
                Genre = theReader.Genre;
                if (null == Genre) { Genre = ""; }
                Year = theReader.IntYear;
                Album = theReader.Album;
                TrackNumber = theReader.Track;
                DiscNumber = theReader.Disc;
                Bitrate = theReader.IntBitRate;
                CodecFamily = theReader.CodecFamily;
                Duration = theReader.IntDuration;
                Rating = theReader.Rating;
                IsVBR = theReader.IsVBR;
                Pictures = new List<MetaReaderFactory.PIC_CODE>(theReader.Pictures);
            }
        }

        protected void Update(MetaDataIOFactory.PictureStreamHandlerDelegate pictureStreamHandler = null)
        {
            FileInfo theFileInfo = new FileInfo(Path);

            if (theFileInfo.Exists)
            {
                Size = theFileInfo.Length;
                LastModified = theFileInfo.LastWriteTime.Ticks;
            }

            //TODO when tag is not available, customize by naming options // tracks (...)
            AudioFileIO theReader = new AudioFileIO(Path, pictureStreamHandler);

            // Per convention, the presence of a pictureStreamHandler
            // indicates that we only want coverArt updated
            if (null == pictureStreamHandler)
            {
                Title = theReader.Title;
                if ("" == Title || null == Title)
                {
                    Title = System.IO.Path.GetFileNameWithoutExtension(Path);
                }
                Artist = theReader.Artist;
                if (null == Artist) { Artist = ""; }
                Composer = theReader.Composer;
                if (null == Composer) { Composer = ""; }
                Comment = theReader.Comment;
                if (null == Comment) { Comment = ""; }
                Genre = theReader.Genre;
                if (null == Genre) { Genre = ""; }
                Year = theReader.IntYear;
                Album = theReader.Album;
                TrackNumber = theReader.Track;
                DiscNumber = theReader.Disc;
                Bitrate = theReader.IntBitRate;
                CodecFamily = theReader.CodecFamily;
                Duration = theReader.IntDuration;
                Rating = theReader.Rating;
                IsVBR = theReader.IsVBR;
                PictureTokens = new List<MetaDataIOFactory.PIC_TYPE>(theReader.Pictures);
            }
        }
	}
}
