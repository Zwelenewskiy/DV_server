declare @input xml = '<ROOT><Id Value="0"/><Id Value="1"/><Id Value="2"/><Id Value="3"/><Id Value="8"/></ROOT>'

select 
	n.p_id,
	a.[xml]
from 
	(
	select 
		n.t.value('@Value[1]', 'nvarchar(max)') as 'p_id'
	from
		@input.nodes('/ROOT/Id') n(t)
	) n(p_id)
CROSS APPLY
(
	select
		s.rowID as 'rowID',
		s.name as 'name'
	from
		source s
	where
		s.parentTreeRowID = n.p_id
	for xml path(''), root('ROOT'), type
		
) a([xml])