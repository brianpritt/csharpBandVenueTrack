using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using BandTracker.Objects;

namespace  BandTracker
{
  public class VenueTest : IDisposable
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
      int result = Venue.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Save_SavesToDb_True()
    {
      //Arrange
      Venue newVenue = new Venue("Akron Agora", "3308698686");
      newVenue.Save();
      //Act
      List<Venue> foundVenues = Venue.GetAll();
      //Assert
      Assert.Equal(1,foundVenues.Count);
    }
    [Fact]
    public void Find_FindsVenueInDB_True()
    {
      //Arrange
      Venue newVenue = new Venue("AKron Agora", "3308698686");
      newVenue.Save();
      Venue foundVenue = Venue.Find(newVenue.GetId());
      //Assert
      Assert.Equal(foundVenue, newVenue);
    }
    [Fact]
    public void Update_UpdatesVenueInDb_True()
    {
      //Arrange
      Venue newVenue = new Venue("AKron Agora", "3308698686");
      newVenue.Save();
      //Act
      newVenue.Edit("Peabodys", "3307625903");
      Venue editedVenue = Venue.Find(newVenue.GetId());
      //Assert
      Assert.Equal("Peabodys", editedVenue.GetName());
    }
    [Fact]
    public void Delete_DeletesVenueFromDb_True()
    {
      //Arrange
      Venue newVenue = new Venue("Akron Agora", "3308698686");
      newVenue.Save();
      //Act
      Venue.Delete(newVenue.GetId());
      List<Venue> foundVenues = Venue.GetAll();
      //Assert
      Assert.Equal(0,foundVenues.Count);
    }
    [Fact]
    public void AddBand_MakesBandVenueRelationship_True()
    {
      //Arrange
      Venue newVenue = new Venue("Akron Agora", "3308698686");
      newVenue.Save();
      Band newBand = new Band("Misery Jackyls", "3308698686");
      newBand.Save();
      //Act
      newVenue.AddBand(newBand);
      List<Band> result = newVenue.GetBands();
      List<Band> testList = new List<Band>{newBand};
      //Assert
      Assert.Equal(testList, result);
    }

    public void Dispose()
    {
      Venue.DeleteAll();
    }
  }
}
