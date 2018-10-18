UPDATE correspondence
SET correspondence.HTMLFullBody = line.HTMLFullBody
FROM Correspondences correspondence
INNER JOIN InboundEmailLines line
    ON correspondence.Id = line.EntityLineId
WHERE correspondence.HTMLFullBody is null