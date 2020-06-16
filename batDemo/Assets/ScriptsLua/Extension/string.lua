string.split = function(str, pat)
   local t = {}
   local fpat = "(.-)" .. pat
   local last_end = 1
   local s, e, cap = str:find(fpat, 1)
   while s do
      if s ~= 1 or cap ~= "" then
         table.insert(t,cap)
      end
      last_end = e + 1
      s, e, cap = str:find(fpat, last_end)
   end
   if last_end <= #str then
      cap = str:sub(last_end)
      table.insert(t, cap)
   end
   return t
end

-- 以某个字符分割字符串
string.strSplit = function(splitStr, splitChar)
    if(not splitStr)then
        return nil
    end
    local startIdx = 1
    local splitIdx = 1
    local splitArr = {}
    while true do
        local lastIdx = string.find(splitStr, splitChar, startIdx)
        if not lastIdx then
            splitArr[splitIdx] = string.sub(splitStr, startIdx, string.len(splitStr))
            break
        end
        splitArr[splitIdx] = string.sub(splitStr, startIdx, lastIdx - 1)
        startIdx = lastIdx + string.len(splitChar)
        splitIdx = splitIdx + 1
    end
    return splitArr
end

-- 正则匹配去掉字符串首尾空格
string.trimStrSpace = function(str)
    if(not str)then return nil end
    return string.gsub(str, "^%s*(.-)%s*$", "%1")
end

string.isNilOrEmpty = function(str)
    return str == nil or str == ""
end

string.lenByte = function(str)
    if(not str)then return nil end
    return (utf8.len(str)+string.len(str))/2
end