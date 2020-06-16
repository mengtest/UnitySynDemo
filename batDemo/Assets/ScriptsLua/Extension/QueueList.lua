local QueueList = {}

function QueueList.New( )
    return {first = 0,last = -1}
end

function QueueList.isEmpty(list)
    if list.first > list.last  then
        return true
    end

    return  false
end

function QueueList.pushfirst(list,value)
    local first = list.first - 1
    list.first = first
    list[first] = value
end

function QueueList.pushlast(list,value)
    local last = list.last + 1
    list.last = last
    list[last] = value
end

function QueueList.getfirst(list)
    local first = list.first
    if first > list.last  then
        log.Log("list is empty")
        return nil
    end
    return list[first]
end

function QueueList.getLast(list)
    local last = list.last
    if list.first > last  then
        log.Log("list is empty")
        return nil
    end
    return list[last]
end

function QueueList.popfirst(list)
    local first = list.first
    if first > list.last  then
        log.Log("list is empty")
        return nil
    end
    local value = list [first]
    list[first] = nil
    list.first = first + 1
    return value
end

function QueueList.poplast(list)
    local last = list.last
    if list.first > last  then
        log.Log("list is empty")
        return nil
    end
    local value = list[last]
    list[last] = nil
    list.last = last - 1
    return value
end

return QueueList