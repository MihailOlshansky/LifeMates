﻿namespace LifeMates.WebApi.Controllers.v0.Models.Chat.GetMetches;

public class GetMatchesRequest : IPagination
{
    public int Offset { get; set; }
    public int Limit { get; set; }
}