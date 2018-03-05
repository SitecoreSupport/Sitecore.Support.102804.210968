﻿using Sitecore.Data.Items;
using Sitecore.Data.SqlServer;
using Sitecore.Diagnostics;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sitecore.Support.Data.SqlServer
{
  public class SqlServerLinkDatabase : Sitecore.Data.SqlServer.SqlServerLinkDatabase
  {
    public SqlServerLinkDatabase(string connectionString) : base(connectionString) { }

    public override void UpdateItemVersionReferences(Item item)
    {
      Assert.ArgumentNotNull(item, "item");
      ItemLink[] allLinks = item.Links.GetAllLinks(false);
      this.UpdateItemVersionLink(item, allLinks);
    }

    protected override void AddLink(Item item, ItemLink link)
    {
      Assert.ArgumentNotNull(item, "item");
      Assert.ArgumentNotNull(link, "link");
      string sql = " INSERT INTO {0}Links{1}(   {0}SourceDatabase{1}, {0}SourceItemID{1}, {0}SourceLanguage{1}, {0}SourceVersion{1}, {0}SourceFieldID{1}, {0}TargetDatabase{1}, {0}TargetItemID{1}, {0}TargetLanguage{1}, {0}TargetVersion{1}, {0}TargetPath{1} ) VALUES(  {2}database{3}, {2}itemID{3}, {2}sourceLanguage{3}, {2}sourceVersion{3}, {2}fieldID{3}, {2}targetDatabase{3}, {2}targetID{3}, {2}targetLanguage{3}, {2}targetVersion{3}, {2}targetPath{3} )";
      this.DataApi.Execute(sql, "itemID", item.ID.ToGuid(), "database", this.GetString(item.Database.Name, 150), "fieldID", link.SourceFieldID.ToGuid(), "sourceLanguage", this.GetString(link.SourceItemLanguage.ToString(), 50), "sourceVersion", link.SourceItemVersion.Number, "targetDatabase", this.GetString(link.TargetDatabaseName, 150), "targetID", link.TargetItemID.ToGuid(), "targetLanguage", this.GetString(link.TargetItemLanguage.ToString(), 50), "targetVersion", link.TargetItemVersion.Number, "targetPath", link.TargetPath);
    }
  }
}