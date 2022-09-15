﻿namespace Ordering.Data.Mongo.Configurations
{
  public class OrderDatabaseSettings
  {
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string OrdersCollectionName { get; set; } = null!;
  }
}
