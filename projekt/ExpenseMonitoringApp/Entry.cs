﻿using System;

public class Entry
{
    public long Id { get; set; }
    public long CategoryId { get; set; }
    public decimal MoneyCount { get; set; }
    public long MoneyTypeId { get; set; }
    public long? CommentId { get; set; }
    public DateTime CreationTime { get; set; }
    public Category Category { get; set; }
    public MoneyType MoneyType { get; set; }
    public Comment Comment { get; set; }

    public Entry(long categoryId, decimal moneyCount, long moneyTypeId, DateTime creationTime, long? commentId = null)
    {
        CategoryId = categoryId;
        this.MoneyCount = moneyCount;
        MoneyTypeId = moneyTypeId;
        CreationTime = creationTime;
        CommentId = commentId;
    }
}