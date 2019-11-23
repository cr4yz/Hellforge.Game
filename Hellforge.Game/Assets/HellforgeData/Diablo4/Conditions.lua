function IsUsingOneHander(character)
	for k,v in pairs(character.HellforgeCharacter.Items) do
		print("thing: " .. v)
	end
end

function IsMoving(character)
	return character.Controller.IsMoving;
end
