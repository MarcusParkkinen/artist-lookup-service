namespace ArtistLookupService.Converters
{
    public class ArtistDescriptionConverter : StringContentConverter
    {
        public override string JPathToResource => "$..extract";
    }
}
