using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BandTracker.Objects
{
  public class Band
  {
    private string _name;
    private string _contact;
    private int _id;

    public Band(string name, string contact, int id = 0)
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
    public override bool Equals(Object otherBand)
    {
      if(!(otherBand is Band)) return false;
      else
      {
        Band newBand = (Band) otherBand;
        bool idEquality = _id == newBand.GetId();
        bool nameEquality = _name == newBand.GetName();
        bool contactEquality = _contact == newBand.GetContact();

        return(idEquality && nameEquality && contactEquality);
      }
    }

    public override int GetHashCode()
    {
      return _name.GetHashCode();
    }
    public static List<Band> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand ("SELECT * FROM bands;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      List<Band> allBands = new List<Band>{};

      while(rdr.Read())
      {
        int bandId = rdr.GetInt32(0);
        string bandName = rdr.GetString(1);
        string bandContact = rdr.GetString(2);

        Band newBand = new Band(bandName, bandContact, bandId);
        allBands.Add(newBand);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return allBands;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("INSERT INTO bands (band, contact) OUTPUT INSERTED.id VALUES(@Name, @Contact);", conn);
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

    public static Band Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("SELECT band, contact FROM bands WHERE id = @Id;", conn);
      cmd.Parameters.AddWithValue("@Id", id);
      SqlDataReader rdr = cmd.ExecuteReader();

      string band = null;
      string contact = null;
      while(rdr.Read())
      {
        band = rdr.GetString(0);
        contact = rdr.GetString(1);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return new Band(band, contact, id);
    }
    public void AddVenue(Venue newVenue)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO bands_venues (venue_id, band_id) VALUES (@VenueId, @BandId);", conn);
      cmd.Parameters.AddWithValue("@VenueId", newVenue.GetId());
      cmd.Parameters.AddWithValue("@BandId", this.GetId());

      cmd.ExecuteNonQuery();

      if (conn != null) conn.Close();

    }

    public List<Venue> GetVenues()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT venues.* FROM bands JOIN bands_venues ON (bands.id = bands_venues.band_id) JOIN venues ON (bands_venues.venue_id = venues.id) WHERE bands.id = @VenueId;", conn );
      cmd.Parameters.AddWithValue("@VenueId", this.GetId().ToString());

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Venue> venues = new List<Venue>{};

      while(rdr.Read())
      {
        int venueId = rdr.GetInt32(0);
        string venueDescription = rdr.GetString(1);
        string venueContact = rdr.GetString(2);
        Venue newVenue = new Venue(venueDescription ,venueContact, venueId);
        venues.Add(newVenue);
      }

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return venues;
     }
     public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM bands WHERE id = @BandId; DELETE FROM bands_venues WHERE band_id = @BandId", conn);
      cmd.Parameters.AddWithValue("@BandId", this.GetId());
      cmd.ExecuteNonQuery();

      if(conn!=null) conn.Close();
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM bands;", conn);
      cmd.ExecuteNonQuery();
      if (conn != null) conn.Close();
    }

  }
}
