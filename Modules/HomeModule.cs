using Nancy;
using System.Collections.Generic;
using System;
using BandTracker.Objects;

namespace BandTracker
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Get["/add/band"] = _ => {
        List<Band> allBands = Band.GetAll();
        return View["band_form.cshtml",allBands];
      };
      Post["/add/band"] = _ => {
        Band newBand = new Band(Request.Form["band-name"],Request.Form["contact"]);
        newBand.Save();
        return View["bands.cshtml", Band.GetAll()];
      };
      Get["/band/{id}"] = parameters =>{
        Dictionary<string, object> bandDict = new Dictionary<string, object>();
        var currentBand = Band.Find(parameters.id);
        bandDict.Add("band", currentBand);
        List<Venue> allVenues = Venue.GetAll();
        bandDict.Add("venues", allVenues);
        List<Venue> bandVenues = currentBand.GetVenues();
        bandDict.Add("venuesPlayed",bandVenues);
        return View["band.cshtml", bandDict];
      };
      Get["/add/band/{id}/venue"] = parameters => {
        Band currentBand = Band.Find(parameters.id);
        List<Venue> allVenues = Venue.GetAll();
        Dictionary<string, object> bandVenues = new Dictionary<string, object>();
        bandVenues.Add("band", currentBand);
        bandVenues.Add("venues", allVenues);
        return View["add_venue_to_band.cshtml", bandVenues];
      };
      Post["/add/band/{id}/venue"] = parameters => {
        Dictionary<string, object> bandDict = new Dictionary<string, object>();
        Band currentBand = Band.Find(parameters.id);
        int venueMenueItem = int.Parse(Request.Form["venue_id"]);
        Venue currentVenue = Venue.Find(venueMenueItem);
        currentBand.AddVenue(currentVenue);
        bandDict.Add("band", currentBand);
        List<Venue> allVenues = Venue.GetAll();
        bandDict.Add("venues", allVenues);
        List<Venue> bandVenues = currentBand.GetVenues();
        bandDict.Add("venuesPlayed",bandVenues);
        return View["band.cshtml", bandDict];
      };
      Get["/add/venue"] = _ => {
        List<Venue> allVenues = Venue.GetAll();
        return View["venue_form.cshtml", allVenues];
      };
      Post["/add/venue"] = _ => {
        Venue newVenue = new Venue(Request.Form["venue-name"],Request.Form["contact"]);
        newVenue.Save();
        return View["venues.cshtml", Venue.GetAll()];
      };
      Get["/venue/{id}"] = parameters =>{
        Dictionary<string, object> VenueDict = new Dictionary<string, object>();
        var currentVenue = Venue.Find(parameters.id);
        VenueDict.Add("Venue", currentVenue);
        List<Band> allBands = Band.GetAll();
        VenueDict.Add("bands", allBands);
        List<Band> bandVenues = currentVenue.GetBands();
        VenueDict.Add("bandsPlayed",bandVenues);
        return View["venue.cshtml", VenueDict];
      };
    }
  }
}
