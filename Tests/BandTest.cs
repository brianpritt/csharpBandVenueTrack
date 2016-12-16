using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using BandTracker.Objects;

namespace  BandTracker
{
  public class BandTest : IDisposable
  {
    public void StudentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void GetAll_ReturnsEmpty_true()
    {
      //Arrange
      //Act
      int result = Band.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Save_SavesToDb_True()
    {
      //Arrange
      Band newBand = new Band("Misery Jackyls", "3308698686");
      newBand.Save();
      //Act
      List<Band> foundBands = Band.GetAll();
      //Assert
      Assert.Equal(1,foundBands.Count);
    }
    [Fact]
    public void Find_FindsBandInDB_True()
    {
      //Arrange
      Band newBand = new Band("Misery Jackyls", "3308698686");
      newBand.Save();
      Band foundBand = Band.Find(newBand.GetId());
      //Assert
      Assert.Equal(foundBand, newBand);
    }
    [Fact]
    public void AddVenue_MakesBandVenueRelationship_True()
    {
      //Arrange
      Venue newVenue = new Venue("Akron Agora", "3308698686");
      newVenue.Save();
      Band newBand = new Band("Misery Jackyls", "3308698686");
      newBand.Save();
      //Act
      newBand.AddVenue(newVenue);
      List<Venue> result = newBand.GetVenues();
      List<Venue> testList = new List<Venue>{newVenue};
      //Assert
      Assert.Equal(testList, result);
    }
    public void Dispose()
    {
      Band.DeleteAll();
      Venue.DeleteAll();
    }
  }
}
