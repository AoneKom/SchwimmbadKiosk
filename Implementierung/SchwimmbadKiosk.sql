SELECT * FROM Benutzer;
SELECT * FROM Inventar;
SELECT * FROM Verk�ufe;
SELECT 
    FORMAT(VerkaufDatum, 'yyyy-MM-dd') AS Datum,
    SUM(Gesamtpreis) AS Gesamtumsatz
FROM Verk�ufe
GROUP BY FORMAT(VerkaufDatum, 'yyyy-MM-dd')
ORDER BY Datum DESC;







