using AutoMapper;
using System.Linq;
using vega.Controllers.Resources;
using vega.Models;
using System.Collections.Generic;

namespace vega.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to API Resource
            CreateMap<Make, MakeResource>();
            CreateMap<Model, ModelResource>();
            CreateMap<Feature, FeatureResource>();
            CreateMap<Vehicle, VehicleResource>()
                .ForMember(vr => vr.Contact, opt => opt.MapFrom(v => new ContactResource { Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone }))
                .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => vf.FeatureId)));

            // API Resource to Domain
            CreateMap<VehicleResource, Vehicle>()
                .ForMember(v => v.Id, opt => opt.Ignore())
                .ForMember(v => v.ContactName, opt => opt.MapFrom(vr => vr.Contact.Name))
                .ForMember(v => v.ContactEmail, opt => opt.MapFrom(vr => vr.Contact.Email))
                .ForMember(v => v.ContactPhone, opt => opt.MapFrom(vr => vr.Contact.Phone))
                .ForMember(v => v.Features, opt => opt.Ignore())
                .AfterMap((vr, v) => {
                    // Remove unselected features --> "v" contains feature that was just removed from "vr"
                    var removedFeatures = v.Features.Where(f => !vr.Features.Contains(f.FeatureId));  // started with Select (the result was IEnumerable of boolean then we replaced it with where)
                    foreach (var f in removedFeatures)
                    {
                        v.Features.Remove(f);
                    }

                    // Add new features --> a new feature was just added to "vr" so we need to add it to "v" as well
                    var addedFeatures = vr.Features.Where(id => !v.Features.Any(f => f.FeatureId == id));  // started with Select (the result was IEnumerable of boolean then we replaced it with where)
                    foreach (var id in addedFeatures)
                    {
                        v.Features.Add(new VehicleFeature { FeatureId = id });
                    }
                });
        }
    }
}