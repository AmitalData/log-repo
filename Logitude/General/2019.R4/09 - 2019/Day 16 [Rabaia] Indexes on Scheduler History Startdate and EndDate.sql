CREATE NONCLUSTERED INDEX IX_StartDateTime
   ON TaskSchedulerHistory ([StartDateTime] ASC);

   CREATE NONCLUSTERED INDEX IX_EndDateTime
   ON TaskSchedulerHistory ([EndDateTime] ASC);