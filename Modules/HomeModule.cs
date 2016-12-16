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
        var currentBand = Band.Find(parameters.id);
        return View["band.cshtml", currentBand];
      };

      Get["/add/venue"] = _ => {
        List<Venue> allVenues = Venue.GetAll();
        return View["venue_form.cshtml", allVenues];
      };

    }
  }
}
