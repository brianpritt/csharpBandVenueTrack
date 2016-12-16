using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BandTracker.Objects
{
  public class Venue
  {
    private string _name;
    private string _contact;
    private int _id;

    public Venue(string name, string contact, int id = 0)
    {
      _name = name;
      _contact = contact;
      _id = id;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public string GetContact()
    {
      return _contact;
    }
    public override bool Equals(Object otherVenue)
    {
      if(!(otherVenue is Venue)) return false;
      else
      {
        Venue newVenue = (Venue) otherVenue;
        bool idEquality = _id == newVenue.GetId();
        bool nameEquality = _name == newVenue.GetName();
        bool contactEquality = _contact == newVenue.GetContact();

        return(idEquality && nameEquality && contactEquality);
      }
    }

    public override int GetHashCode()
    {
      return _name.GetHashCode();
    }
    public static List<Venue> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand ("SELECT * FROM venues;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      List<Venue> allVenues = new List<Venue>{};

      while(rdr.Read())
      {
        int VenueId = rdr.GetInt32(0);
        string VenueName = rdr.GetString(1);
        string VenueContact = rdr.GetString(2);

        Venue newVenue = new Venue(VenueName, VenueContact, VenueId);
        allVenues.Add(newVenue);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return allVenues;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("INSERT INTO venues (venue, contact) OUTPUT INSERTED.id VALUES(@Name, @Contact);", conn);
      cmd.Parameters.AddWithValue("@Name", _name);
      cmd.Parameters.AddWithValue("@Contact", _contact);

      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        _id = rdr.GetInt32(0);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
    }
    public void Edit(string name, string contact)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("UPDATE venues Set venue = @NewName, contact = @NewContact OUTPUT INSERTED.Venue Where id = @VenueId;", conn);
      cmd.Parameters.AddWithValue("@NewName", name);
      cmd.Parameters.AddWithValue("@NewContact", contact);
      cmd.Parameters.AddWithValue("@VenueId", _id);
      SqlDataReader rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        _name = rdr.GetString(0);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
    }
    public static Venue Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("SELECT venue, contact FROM venues WHERE id = @Id;", conn);
      cmd.Parameters.AddWithValue("@Id", id);
      SqlDataReader rdr = cmd.ExecuteReader();

      string venue = null;
      string contact = null;
      while(rdr.Read())
      {
        venue = rdr.GetString(0);
        contact = rdr.GetString(1);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return new Venue(venue, contact, id);
    }

    public void AddBand(Band newBand)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO bands_venues (venue_id, band_id) VALUES (@VenueId, @BandId);", conn);
      cmd.Parameters.AddWithValue("@VenueId",this.GetId());
      cmd.Parameters.AddWithValue("@BandId", newBand.GetId());

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Band> GetBands()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT bands.* FROM venues JOIN bands_venues ON (venues.id = bands_venues.venue_id) JOIN bands ON (bands_venues.band_id = bands.id) WHERE venues.id = @BandId;", conn );
      cmd.Parameters.AddWithValue("@BandId", this.GetId().ToString());

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Band> bands = new List<Band>{};

      while(rdr.Read())
      {
        int bandId = rdr.GetInt32(0);
        string bandDescription = rdr.GetString(1);
        string bandContact = rdr.GetString(2);
        Band newBand = new Band(bandDescription ,bandContact, bandId);
        bands.Add(newBand);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return bands;
     }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("Delete FROM venues Where id = @VenueId; DELETE FROM bands_venues WHERE venue_id = @VenueId", conn);
      cmd.Parameters.AddWithValue("@VenueId", this.GetId());

      cmd.ExecuteNonQuery();
      if (conn != null) conn.Close();
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM venues; ", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

  }
}
