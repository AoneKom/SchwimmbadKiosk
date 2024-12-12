SELECT * FROM Benutzer;
SELECT * FROM Inventar;
SELECT * FROM Verkäufe;
SELECT 
    FORMAT(VerkaufDatum, 'yyyy-MM-dd') AS Datum,
    SUM(Gesamtpreis) AS Gesamtumsatz
FROM Verkäufe
GROUP BY FORMAT(VerkaufDatum, 'yyyy-MM-dd')
ORDER BY Datum DESC;







