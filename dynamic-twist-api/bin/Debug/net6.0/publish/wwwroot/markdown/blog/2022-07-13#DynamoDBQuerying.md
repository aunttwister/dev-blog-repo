Article - 13/7/2022 - 3 minutes to read
# **Working with DynamoDB for the first time**

It&#39;s always exciting to play around with new tech. Unfortunately, we rarely get a chance to play around at our regular jobs.

Fairly recently I actually got a chance to play around with DynamoDB. A bit unconventional for a junior dev, but I negotiated my way into this task. It was a rough negotiation, but I persisted and lo and behold, it was mine.

The outline of the task was straightforward:

1. Pull the data from 3rd party API.
2. Insert data into table/s.

When discussing the task with a team lead, we came to a conclusion that there are some constraints and requirements:

1. Only data that we don&#39;t have in our database should be inserted.
2. Data ingestion should occur couple of times a day.
3. Data will be viewed seldomly.

There&#39;s one golden advice when you&#39;re working with DynamoDB for the first time assuming that you&#39;ve had experience with relational databases:

**Forget everything you know about relational databases.**

DynamoDB has almost no resemblance to relational database way of thinking.

**Querying**

I am not talking about super complex querying here. That can be a difficult and very big select statement can be a real pain. It takes a significant skill to really optimize and make the best performance out of them. Here I&#39;m rather referring to simple select statements. Getting certain columns and rows and ordering them or grouping them does not seem complicated. You have most tools that you need to get whichever data you want.

Initial though process for this task, in order to fulfill point one in constraint and requirements section was to take all the data from the table, order it from latest to newest and take only the data that is missing from the table. Once the data that has been ingested has the same date as last entry in the table, the data ingestion should be stopped.

In DynamoDB there are two types of querying. Table scan and table querying. Table scan is equivalent to the paragraph above, **it queries all data.** This is a big no in DynamoDB as this operation is most expensive operation in this database. This is because DynamoDB is a great database choice if you are doing a lot of data writes. Data writes are cost effective in this case. If you intend to do a lot of data reads, this might not be a good database choice.

At this point we switched gears. We ended up creating a new table with the latest records. Once we insert the data, we took the latest instance and put it in a new latest record table. This made it pretty easy to query the data, as the latest record table had only 3 instance at a time. Partition key was a main table name, so whenever we wanted to get the latest record we would use that table.

**Relations**

Very often non relational databases, as the name suggests, do not have relations between the tables. This is 100% the case in DynamoDB. Since DynamoDB is not really read optimized database, relations would not pose a significant value. And this is a great thing for certain use cases. So if you&#39;re into database modeling this is probably not the most exciting database out there.

**Conclusion**

There&#39;s a great performing tech for almost every use case out there. And if there&#39;s not, it&#39;s probably in the making. This makes it a great pleasure to work on software development as it&#39;s really fun to try, test and use different tech. Exciting times!