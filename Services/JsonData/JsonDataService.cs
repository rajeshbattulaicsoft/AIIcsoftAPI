
using AIIcsoftAPI.HelperClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text;

namespace AIIcsoftAPI.Services.JsonData
{
    public class JsonDataService : IJsonDataService
    {
        public JsonDataService()
        {

        }
        public async Task<string> GetJsonDataAsync(string dataType)
        {

            string ConStr;
            string query = string.Empty;
            DBConfig db = new DBConfig();
            ConStr = db.GetMainConnectionString();
            StringBuilder jsonResult = new StringBuilder();
            switch (dataType)
            {
                case "P":
                    query = "Select p.PONo, Convert(Varchar, p.PODate, 103) PODate,[GRN No],[Invoice No],IsNUll([Voucher No],'') as [Voucher No],[Supplier],Address,CountryName,[Item Category],Itemcode,Item,[Grn/Alt/Qty],Round([Basic Price],2) as [Basic Price],isnull([Tax Type],'') as [Tax Type],Round(isnull(TCD,0),2) as TCD from (select IGM.GrnId,IG.GrnDate as [Date],Grn_Ref_no as [GRN No],ExcInvoiceNo as [Invoice No],Case when transid = convert(varchar,0) then Null else transid end as [Voucher No],PD.Code+' | '+PD.Name as [Supplier],case when IGY.GrnType='Non-Inventory Items' then igm.GrnDesc else  rm.rawmatname+''+igm.GrnDesc end as  Item,RM.RawMatCode Itemcode,IGM.Gqty as [Grn/Alt/Qty],ROUND((GRNBasicPrice-(GRNDiscount+GRNPacking))*IGM.PuomQty,2) as [Basic Price],SUM(Claimedamount) as [Claimed Amount],convert(varchar,TaxCatSlNo)+'.'+TaxCatName as [Tax Type],Round(SUM(IGT.Amount),2) as TCD,ppp.PONo,ppp.PODate,pd.Address,ct.CountryName,igy.GrnType [Item Category] from Invent_grn IG inner join partydetail PD   on Ig.SupID=PD.partyid left join country ct on ct.CountryId=pd.CountryId inner join Invent_grnMaterialDetail IGM   on IGM.GrnNo=IG.GrnNo inner join Rawmaterial Rm   on Rm.RawMatid=IGM.RawmatID inner join Invent_GRNType IGY   on IGY.GrnTypeId = RM.GrnTypeId inner join Invent_GrnTax IGT   on IGT.GrnID=IGM.GrnID inner join Sales_TaxMaster ST  on ST.TaxID=IGT.TaxID inner join TaxCategory TC on TC.TaxCatId = ST.Taxcatid inner join Invent_Purchaseproduct pp on pp.POProductId=IGM.POProductId inner join invent_purchase PPp on ppp.POID = pp.poid where  IG.AddnlParameter in('GRN With PO') and ppp.postatus in('Acknowledged','Closed') Group by IGM.GrnId,IG.GrnDate,Grn_Ref_no,ExcInvoiceNo,PD.Code,PD.Name,Rm.Rawmatcode,RM.Rawmatname,RM.RawMatCode,IGM.PuomQty ,Igm.Gqty,GRNBasicPrice,GRNDiscount,GRNPacking,IGM.PuomQty,transid,TaxCatName,TaxCatSlNo,IGY.GrnType,IGM.GrnDesc,ppp.PONo,ppp.PODate,pd.Address,ct.CountryName,igy.GrnType)P Full join (select IGM.GrnId,SUM(Case When Invoicetaxtype='Excise' then ROUND((Claimedamount),2) Else '' end) as ExAmt,ppp.PONo,ppp.PODate from Invent_grn IG inner join partydetail PD   on Ig.SupID=PD.partyid inner join Invent_grnMaterialDetail IGM   on IGM.GrnNo=IG.GrnNo inner join Rawmaterial Rm   on Rm.RawMatid=IGM.RawmatID inner join Invent_GRNType IGY   on IGY.GrnTypeId = RM.GrnTypeId inner join Invent_GrnTax IGT   on IGT.GrnID=IGM.GrnID inner join Sales_TaxMaster ST  on ST.TaxID=IGT.TaxID inner join TaxCategory TC on TC.TaxCatId = ST.Taxcatid inner join Invent_Purchaseproduct pp on pp.POProductId=IGM.POProductId inner join invent_purchase PPp on ppp.POID = pp.poid where IG.AddnlParameter in('GRN With PO') and ppp.postatus in('Acknowledged','Closed') Group by IGM.GrnId,IG.GrnDate,Grn_Ref_no,ExcInvoiceNo,PD.Code,PD.Name,Rm.Rawmatcode,RM.Rawmatname,RM.RawMatCode,IGM.PuomQty ,GRNBasicPrice,GRNDiscount,GRNPacking,IGM.PuomQty,transid,igm.Gqty,ppp.PONo,ppp.PODate)P1 on p.grnid=P1.grnid";
                    break;

                case "I":
                    query = "Select p.PONo, Convert(Varchar, p.PODate, 103) PODate,[GRN No],[Invoice No],IsNUll([Voucher No],'') as [Voucher No],[Supplier],Address,CountryName,[Item Category],Itemcode,Item,[Grn/Alt/Qty],Round([Basic Price],2) as [Basic Price],isnull([Tax Type],'') as [Tax Type],Round(isnull(TCD,0),2) as TCD from (select IGM.GrnId,IG.GrnDate as [Date],Grn_Ref_no as [GRN No],ExcInvoiceNo as [Invoice No],Case when transid = convert(varchar,0) then Null else transid end as [Voucher No],PD.Code+' | '+PD.Name as [Supplier],case when IGY.GrnType='Non-Inventory Items' then igm.GrnDesc else  rm.rawmatname+''+igm.GrnDesc end as  Item,RM.RawMatCode Itemcode,IGM.Gqty as [Grn/Alt/Qty],ROUND((GRNBasicPrice-(GRNDiscount+GRNPacking))*IGM.PuomQty,2) as [Basic Price],SUM(Claimedamount) as [Claimed Amount],convert(varchar,TaxCatSlNo)+'.'+TaxCatName as [Tax Type],Round(SUM(IGT.Amount),2) as TCD,ppp.PONo,ppp.PODate,pd.Address,ct.CountryName,igy.GrnType [Item Category] from Invent_grn IG inner join partydetail PD   on Ig.SupID=PD.partyid left join country ct on ct.CountryId=pd.CountryId inner join Invent_grnMaterialDetail IGM   on IGM.GrnNo=IG.GrnNo inner join Rawmaterial Rm   on Rm.RawMatid=IGM.RawmatID inner join Invent_GRNType IGY   on IGY.GrnTypeId = RM.GrnTypeId inner join Invent_GrnTax IGT   on IGT.GrnID=IGM.GrnID inner join Sales_TaxMaster ST  on ST.TaxID=IGT.TaxID inner join TaxCategory TC on TC.TaxCatId = ST.Taxcatid inner join Invent_Purchaseproduct pp on pp.POProductId=IGM.POProductId inner join invent_purchase PPp on ppp.POID = pp.poid where  IG.AddnlParameter in('GRN With PO') and ppp.postatus in('Acknowledged','Closed') Group by IGM.GrnId,IG.GrnDate,Grn_Ref_no,ExcInvoiceNo,PD.Code,PD.Name,Rm.Rawmatcode,RM.Rawmatname,RM.RawMatCode,IGM.PuomQty ,Igm.Gqty,GRNBasicPrice,GRNDiscount,GRNPacking,IGM.PuomQty,transid,TaxCatName,TaxCatSlNo,IGY.GrnType,IGM.GrnDesc,ppp.PONo,ppp.PODate,pd.Address,ct.CountryName,igy.GrnType)P Full join (select IGM.GrnId,SUM(Case When Invoicetaxtype='Excise' then ROUND((Claimedamount),2) Else '' end) as ExAmt,ppp.PONo,ppp.PODate from Invent_grn IG inner join partydetail PD   on Ig.SupID=PD.partyid inner join Invent_grnMaterialDetail IGM   on IGM.GrnNo=IG.GrnNo inner join Rawmaterial Rm   on Rm.RawMatid=IGM.RawmatID inner join Invent_GRNType IGY   on IGY.GrnTypeId = RM.GrnTypeId inner join Invent_GrnTax IGT   on IGT.GrnID=IGM.GrnID inner join Sales_TaxMaster ST  on ST.TaxID=IGT.TaxID inner join TaxCategory TC on TC.TaxCatId = ST.Taxcatid inner join Invent_Purchaseproduct pp on pp.POProductId=IGM.POProductId inner join invent_purchase PPp on ppp.POID = pp.poid where IG.AddnlParameter in('GRN With PO') and ppp.postatus in('Acknowledged','Closed') Group by IGM.GrnId,IG.GrnDate,Grn_Ref_no,ExcInvoiceNo,PD.Code,PD.Name,Rm.Rawmatcode,RM.Rawmatname,RM.RawMatCode,IGM.PuomQty ,GRNBasicPrice,GRNDiscount,GRNPacking,IGM.PuomQty,transid,igm.Gqty,ppp.PONo,ppp.PODate)P1 on p.grnid=P1.grnid";
                    break;

                case "E":
                    query = "Select   isnull(convert(varchar,invdate1,105),'') [Date],isnull(invoiceno1,'') [Invoice NO],Custcode As [Customer Code],Custname As [Customer Name],Address,CountryName,[Item Category],rawmatcode,RawMatName,Despatchqty,isnull( round(BasicCost,0),'') [Basic Cost],isnull([Currency Type],'') as [Currency Type],isnull(ExRate,'') as ExRate,IsNull(Roundoff,0) Roundoff , isnull (taxtype1,'') [Tax Type],case when convert(varchar,taxamount) IS NULL then '0' else isnull(convert(varchar,round(taxamount,2)),'0') end as [Tax] from (Select i.invdate1 ,i.invoiceno1,i.invoiceno,I.BasicCost, Isnull((Tax.taxamount),0) as Taxamount, tax.taxtype,tax.taxtype as taxtype1,tax.RoundOff As Roundoff,Code as CustCode ,Name as CustName ,DefaultValue,Istax,ExRate,[Currency Type],GSTNO,CountryName,Despatchqty,Address,RawMatName,RawMatCode,GrnType [Item Category] From (Select i.invdate1,i.invoiceno1,i.InvoiceVatNo,i.invoiceno, Sum(Case When (Despatchqty)<>0 Then Round(((Despatchqty * (Price-(Disc+Pack))*I.ExRate)),0) Else ((Price-(Disc+Pack))*I.ExRate) End) BasicCost,c.Code ,c.Name, isnull(I.ExRate,'')  as ExRate,isnull(CM.CurrCode,'') as [Currency Type],C.GST as GSTNO ,ct.CountryName,d.Despatchqty,c.Address,r.RawMatName,r.RawMatCode,igt.GrnType From Invoice I Inner Join Despatch D   On I.Invoiceno=D.Invoiceno inner join PartyDetail C   on c.PartyID = I.custid INNER JOIN invent_rawmatlocation ON invent_rawmatlocation.Location_ID=I.storagelocid left outer join Currencymaster CM   on I.CurrID = CM.currid left join country ct on ct.CountryId=c.CountryId inner join rawmaterial r on r.RawMatID=d.ProdId inner join invent_grntype igt on igt.GrnTypeId=r.GrnTypeId Where     invdone='Y' and parentid=0 Group By .invdate1,i.invoiceno1,i.invoiceno,c.Code ,c.Name,i.InvoiceVatNo,I.ExRate,CM.CurrCode,C.GST,ct.CountryName,d.Despatchqty,c.Address,r.RawMatName,r.RawMatCode,igt.GrnType) I left outer join (select i.invdate1,i.invoiceno1,I.invoiceno,isnull(sum(SI.amount),0) as Taxamount ,Convert(varchar,ST.Code)+''+case when Perc ='Y' then Isnull(Convert(Varchar,SI.DefValue),'')+'%' +' '+ +' '+ TaxType when Perc ='N' then IsNull(Convert(Varchar,SI.DefValue),'')+'' +' '+ +' '+ TaxType end as taxtype,taxcatid ,I.RoundOff,si.DefValue as DefaultValue,ST.Istax from invoice I inner join sales_invoicetaxdetails SI on I.invoiceno = SI.invno inner join sales_taxmaster ST   on ST.taxid = SI.taxid inner join PartyDetail C1   on C1.PartyID =I.CustID INNER JOIN invent_rawmatlocation ON invent_rawmatlocation.Location_ID=I.storagelocid where invdone='Y' group by i.invdate1,i.invoiceno1,I.invoiceno ,taxtype,taxcatid ,I.RoundOff ,si.DefValue,ST.Istax,ST.code,ST.perc)Tax on I.invoiceno1 = tax.invoiceno1 UNION All Select DOCDate,BillNo,a,BasicCost ,TaxAmount,TaxType,taxtype1,Roundoff,accountcode,Name,DefaultValue,Istax,ExRate,[Currency Type],GSTNO,CountryName,Despatchqty,Address,RawMatName,rawmatcode,GrnType From (Select T.DOCDate,T.BillNo,T.BillNo As Invoicevatno,'' as a,Isnull((Select Sum(Amount) From [KrossLedger].dbo.transactions A where A.transactionnumber=T.transactionnumber And A.TrTypeno In(23,40) And DrCr='Cr' And TaxId=0 ),0)As BasicCost ,T.Amount As TaxAmount,ST.TaxType,Convert(varchar,ST.Code)+''+Isnull(Convert(Varchar,DefaultValue),'') +''+ +':'+taxtype  as taxtype1,T.Taxableamount As Roundoff,D.accountcode,D.Name,SI.DefValue as DefaultValue,ST.Istax,isnull(CM.ExRate,'') as ExRate,isnull(CM.CurrCode,'') as [Currency Type],IsNull((Select Sum(CurrencyAmount) From [KrossLedger].dbo.transactions A where A.transactionnumber=T.transactionnumber And A.TrTypeno In(23,40) And DrCr='Cr' And TaxId=0 ),0)As ,isnull(p.GST,'') ,ct.CountryName,ds.Despatchqty,p.Address,r.RawMatName,r.RawMatCode,igt.GrnType from [KrossLedger].dbo.Accounts_Transactions_Query T INNER JOIN sales_taxmaster ST   ON T.TaxId=ST.TaxId LEFT OUTER JOIN (Select transactionnumber,Name,accountcode from KrossLedger].dbo.Accounts_Transactions_Query where drcr='Dr')D ON D.transactionnumber=t.transactionnumber left outer join Currencymaster CM   on t.CurrID = CM.currid Left outer join PartyDetail p  on p.AccId=t.AccountID left join country ct on ct.CountryId=p.CountryId Inner join Invoice I   on I.InvoiceNo1 =T.BillNo Inner Join Despatch Ds   On I.Invoiceno=Ds.Invoiceno inner join rawmaterial r on r.RawMatID=ds.ProdId inner join invent_grntype igt on igt.GrnTypeId=r.GrnTypeId inner join sales_invoicetaxdetails SI    on sI.invoiceno = I.InvoiceNo where T.trtypeno =23  )A)X";
                    break;

                default:
                    break;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(ConStr))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                jsonResult.Append(reader.GetString(0));
                            }

                        }
                    }


                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return jsonResult.ToString();

        }
    }
}
