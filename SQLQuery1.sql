
 o.menuId;
 query = "select c.orderId, o.menuId, c.generatedTimeDate, d.percentage, p.amount, p.payType " +
                              "from CustomerOrder c inner join Discount d on c.discountId = d.discountId " +
                                  "inner join PaymentBill p on p.billId = c.billId " +
                                  "inner join OrderedMenus o on o.orderId = c.orderId " +
                                  "inner join Menus m on m.menuId = o.menuId " +
                              $" Where c.generatedTimeDate between '{dtFromDate.Text}' and '{dtToDate.Text}' " +
                              " group by c.orderId,  o.menuId, c.generatedTimeDate, d.percentage,p.amount, p.payType ";

select c.orderId, o.menuId, c.generatedTimeDate, d.percentage, p.amount, p.payType 
	from CustomerOrder c inner join Discount d on c.discountId=d.discountId 
				inner join PaymentBill p on p.billId=c.billId
				inner join OrderedMenus o on o.orderId=c.orderId
				inner join Menus m on m.menuId=o.menuId
	--Where c.orderId = 1 
	group by c.orderId, o.menuId, c.generatedTimeDate,  
		d.percentage,p.amount, p.payType ;



select c.orderId, o.menuId, m.menuName, o.quantity, m.price 
	from CustomerOrder c  inner join Discount d on c.discountId=d.discountId 
				inner join PaymentBill p on p.billId=c.billId
				inner join OrderedMenus o on o.orderId=c.orderId
				inner join Menus m on m.menuId=o.menuId 
	Where c.orderId = 1;


select c.orderId, c.generatedTimeDate,d.percentage,p.amount,   -- c.billId,
			p.payType    --,o.menuId,m.menuName,o.quantity,m.price 
	from CustomerOrder c inner join Discount d on c.discountId=d.discountId 
						 inner join PaymentBill p on p.billId=c.billId
						 inner join OrderedMenus o on o.orderId=c.orderId
						 inner join Menus m on m.menuId=o.menuId
    group by c.orderId,  c.generatedTimeDate,d.percentage,p.amount, p.payType ;


-- Pooja
select o.orderId, c.billId,c.generatedTimeDate,d.percentage,p.amount,
			p.payType,o.menuId,m.menuName,o.quantity,m.price 
	from CustomerOrder c inner join Discount d on c.discountId=d.discountId 
						 inner join PaymentBill p on p.billId=c.billId
						 inner join OrderedMenus o on o.orderId=c.orderId
						 inner join Menus m on m.menuId=o.menuId;



select * from CustomerOrder;

select * from PaymentBill;