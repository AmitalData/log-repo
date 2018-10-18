-- Apply it in Main DB
update ComputingPartners set Code ='G-'+Code  where Tenant=0 and Code Not Like 'G-%'; 