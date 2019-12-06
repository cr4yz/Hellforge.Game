function IsUsingOneHander(hero)
	return false;
end

function IsMoving(hero)
	return hero.Controller.IsMoving;
end

function HasMinimumAttribute(hero, attributeName, amount)
	return hero.Character:GetAttribute(attributeName) >= tonumber(amount)
end

