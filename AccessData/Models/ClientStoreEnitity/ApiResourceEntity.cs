using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AccessData.Models.ClientStoreEnitity
{
    public class ApiResourceEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string DisplayName { get; set; }

        public bool Enabled { get; set; }

        [NotMapped]
        public ApiResource ApiResource { get; set; }

        public void AddDataToEntity()
        {
            Description = ApiResource.Description;
            DisplayName = ApiResource.DisplayName;
            Enabled = ApiResource.Enabled;
            Name = ApiResource.Name;
        }

        public void MapDataFromEntity()
        {
            ApiResource = new ApiResource();
            ApiResource.Description = Description;
            ApiResource.DisplayName = DisplayName;
            ApiResource.Enabled = Enabled;
            ApiResource.Name = Name;
        }
    }
}
