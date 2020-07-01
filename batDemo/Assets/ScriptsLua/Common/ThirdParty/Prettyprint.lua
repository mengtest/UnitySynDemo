----------------------------------------
-- print() replacement that will pretty print tables
--
-- @module prettyprint
-- @author Björn Ritzl
-- @license Apache License 2.0
-- @github https://github.com/britzl/prettyprint
----------------------------------------


local pp = {
	print = print,
	indentation = "\t",
	-- list of keys to ignore when printing tables
	ignore = nil,
}

-- keep track of printed tables to avoid infinite loops
-- when tables refer to each other
local printed_tables = {}

local current_indentation = ""

--- Check if a name is ignore
-- Ignored names are configured in pp.ignore
-- @param name
-- @return true if name exists in table of ignored names
local function is_ignored(name)
	if not pp.ignore then
		return false
	end
	for _,ignored in pairs(pp.ignore) do
		if name == ignored then
			return true
		end
	end
	return false
end

local function set_ignore( ignoreTable )
	pp.ignore = ignoreTable
end

local function clean_ignore( )
	pp.ignore = nil
end

local function indent_more()
	current_indentation = current_indentation .. pp.indentation
end

local function indent_less()
	current_indentation = current_indentation:sub(1, #current_indentation - #pp.indentation)
end

--- Format a table into human readable output
-- For every line in the human readable output a callback will be invoked. If no
-- callback is specified print() will be used.
-- @param value The value to convert into a human readable format
-- @param callback Function to call for each line of human readable output
function pp.format_table(value, isFull, callback)
	callback = callback or pp.print

	callback(current_indentation .. "{")
	indent_more()

	for name,data in pairs(value) do
		if not is_ignored(name) then
			if type(name) == "string" then
				name = '"'..name..'"'
			else
				name = tostring(name)
			end
			local dt = type(data)
			if dt == "table" then
				callback(current_indentation .. name .. " = [".. tostring(data) .. "]")
				if not printed_tables[data] then
					printed_tables[data] = true
					pp.format_table(data, isFull, callback)
				end
			elseif dt == "string" then
				callback(current_indentation .. name .. ' = "' .. tostring(data) .. '"')
			else
				callback(current_indentation .. name .. " = " .. tostring(data))
			end
		end
	end
	indent_less()
	callback(current_indentation .. "}")
end

--- Convert value to a human readable string. If the value to convert is a table
-- it will be formatted using format_table() and returned as a string
-- @param value
-- @return String representation of value
function pp.tostring(value, isFull)
	if type(value) ~= "table" then
		return tostring(value)
	end
	local s = ""
	printed_tables = {}
	current_indentation = ""

	clean_ignore()
	if not isFull then
		if value["class"] and value["class"]["name"] then
			local ignoreList = {"class"}
			local className = '\n\t"Class Name" = "' .. value["class"]["name"] .. '"\n\t'
			s = s .. className

			if value["class"]["super"] and value["class"]["super"]["name"] then
				local superName = '"Super Name" = "' .. value["class"]["super"]["name"] .. '"\n\t'
				s = s .. superName

				if value["class"]["super"]["name"] == "BaseData" then
					table.insert( ignoreList, "_instanceDict" )
				end
			end
			set_ignore(ignoreList)
		end
	end

	pp.format_table(value, isFull, function(line)
		s = s .. line .. "\n"
	end)
	return s
end

local function improved_print(isFull, ...)
	-- iterate through each of the arguments and print them one by one
	local args = { ... }
	-- table.remove(args, 1)
	local s = ""
	for _, v in pairs(args) do
		if string.find(s, "\n") == nil then
			s = s .. pp.tostring(v, isFull).." "
		else
			s = s .. pp.tostring(v, isFull) .. "\t"
		end
	end
	return s
end

local prettyPrint = {}

function prettyPrint.getString( ... )
	return improved_print(false, ...)
end

function prettyPrint.getStringAll( ... )
	return improved_print(true, ...)
end

return prettyPrint