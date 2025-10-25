using WeatherApp.Models;
using WeatherApp.Services;
using Xunit;

namespace WeatherApp.Tests.Services
{
    public class RegionInfoServiceTests
    {
        private readonly RegionInfoService _service;

        public RegionInfoServiceTests()
        {
            _service = new RegionInfoService();
        }

        [Fact]
        public void GetAllRegions_ReturnsAllThreeRegions()
        {
            // Act
            var regions = _service.GetAllRegions();

            // Assert
            Assert.NotNull(regions);
            Assert.Equal(3, regions.Count);
            Assert.Contains(regions, r => r.Type == RegionType.Middle);
            Assert.Contains(regions, r => r.Type == RegionType.Lower);
            Assert.Contains(regions, r => r.Type == RegionType.Upper);
        }

        [Theory]
        [InlineData(RegionType.Middle, "Срединный регион")]
        [InlineData(RegionType.Lower, "Нижний регион")]
        [InlineData(RegionType.Upper, "Верхний регион")]
        public void GetRegionInfo_ForEachRegionType_ReturnsCorrectInfo(RegionType regionType, string expectedName)
        {
            // Act
            var regionInfo = _service.GetRegionInfo(regionType);

            // Assert
            Assert.NotNull(regionInfo);
            Assert.Equal(regionType, regionInfo.Type);
            Assert.Equal(expectedName, regionInfo.Name);
            Assert.False(string.IsNullOrEmpty(regionInfo.Description));
            Assert.False(string.IsNullOrEmpty(regionInfo.Characteristics));
            Assert.False(string.IsNullOrEmpty(regionInfo.TypicalWeather));
            Assert.False(string.IsNullOrEmpty(regionInfo.Recommendations));
        }

        [Fact]
        public void GetRegionInfo_ForUnknownRegion_ReturnsEmptyInfo()
        {
            // Arrange
            var unknownRegion = (RegionType)999; // Несуществующий регион

            // Act
            var regionInfo = _service.GetRegionInfo(unknownRegion);

            // Assert
            Assert.NotNull(regionInfo);
            Assert.Equal(default(RegionType), regionInfo.Type);
            Assert.Equal(string.Empty, regionInfo.Name);
        }

        [Theory]
        [InlineData(RegionType.Middle)]
        [InlineData(RegionType.Lower)]
        [InlineData(RegionType.Upper)]
        public void GetRegionName_ForEachRegion_ReturnsNonEmptyString(RegionType regionType)
        {
            // Act
            var regionName = _service.GetRegionName(regionType);

            // Assert
            Assert.False(string.IsNullOrEmpty(regionName));
        }

        [Theory]
        [InlineData(RegionType.Middle)]
        [InlineData(RegionType.Lower)]
        [InlineData(RegionType.Upper)]
        public void GetRegionDescription_ForEachRegion_ReturnsNonEmptyString(RegionType regionType)
        {
            // Act
            var description = _service.GetRegionDescription(regionType);

            // Assert
            Assert.False(string.IsNullOrEmpty(description));
        }
    }
}