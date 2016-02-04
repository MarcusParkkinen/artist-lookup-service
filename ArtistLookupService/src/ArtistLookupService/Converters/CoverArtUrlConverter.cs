namespace ArtistLookupService.Converters
{
    public class CoverArtUrlConverter : StringContentConverter
    {
        public override string JPathToResource => "$..images[?(@.front==true)]..image";
    }
}
