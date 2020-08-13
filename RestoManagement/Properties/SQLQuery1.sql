

select c.billId,c.generatedTimeDate, d.percentage,p.amount,
		p.payType 
	from CustomerOrder c inner join Discount d on c.discountId=d.discountId 
				inner join PaymentBill p on p.billId=c.billId
				--inner join OrderedMenus o on o.orderId=c.orderId
				--inner join Menus m on m.menuId=o.menuId
	Where c.orderId = 1;

select c.billId, m.menuId, c.generatedTimeDate,d.percentage,p.amount, p.payType 
	from CustomerOrder c inner join Discount d on c.discountId=d.discountId 
				inner join PaymentBill p on p.billId=c.billId 
				inner join OrderedMenus o on o.orderId=c.orderId
				inner join Menus m on m.menuId=o.menuId
	Where c.orderId = 1;



select o.menuId,m.menuName,o.quantity,m.price 
	from CustomerOrder c  --inner join Discount d on c.discountId=d.discountId 
			--	inner join PaymentBill p on p.billId=c.billId
				inner join OrderedMenus o on o.orderId=c.orderId
				inner join Menus m on m.menuId=o.menuId 
	Where c.orderId = 1;


	S