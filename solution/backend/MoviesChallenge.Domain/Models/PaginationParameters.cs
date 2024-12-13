﻿namespace MoviesChallenge.Domain.Models;

public class PaginationParameters
{
    private const int MaxPageSize = 1000;
    private int _pageSize = 10;

    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}
