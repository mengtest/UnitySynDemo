TweenManager = {}
TweenFun={}


TweenFun.anchorPosBy = "anchorPosBy";
TweenFun.anchorPosTo = "anchorPosTo";
TweenFun.anchorPosXBy = "anchorPosXBy";
TweenFun.anchorPosXTo = "anchorPosXTo";
TweenFun.anchorPosYBy = "anchorPosYBy";
TweenFun.anchorPosYTo = "anchorPosYTo";
TweenFun.localMoveBy = "localMoveBy";
TweenFun.localMoveTo = "localMoveTo";
--增量移动.
TweenFun.moveBy = "moveBy";
--移动到目标点.
TweenFun.moveTo = "moveTo";
TweenFun.rotateBy = "rotateBy";
TweenFun.rotateTo = "rotateTo";
TweenFun.rotateMBy = "rotateMBy";
TweenFun.rotateMTo = "rotateMTo";
TweenFun.scaleBy = "scaleBy";
TweenFun.scaleTo = "scaleTo";
TweenFun.fadeTo = "fadeTo";
TweenFun.fadeToCG = "fadeToCG";
TweenFun.colorTo = "colorTo";
TweenFun.colorToAll = "colorToAll";
TweenFun.delay = "delay";
TweenFun.call = "call";
TweenFun.update = "update";
TweenFun.to = "to";
TweenFun.spawn = "spawn";
TweenFun.preferredSizeBy = "preferredSizeBy";
TweenFun.preferredSizeTo = "preferredSizeTo";
TweenFun.horizontalNormalizedPosBy = "horizontalNormalizedPosBy";
TweenFun.horizontalNormalizedPosTo = "horizontalNormalizedPosTo";
TweenFun.verticalNormalizedPosBy = "verticalNormalizedPosBy";
TweenFun.verticalNormalizedPosTo = "verticalNormalizedPosTo";
TweenFun.normalizedPosBy = "normalizedPosBy";
TweenFun.normalizedPosTo ="normalizedPosTo";
TweenFun.punchPosition ="punchPosition";
TweenFun.punchRotation="punchRotation";
TweenFun.punchScale="punchScale";


-- 初始化
function TweenManager.init()
	TweenManager.tweenMap = {}
end

-- 执行动画
function TweenManager.tween( gameObj, tweening )
	if Utils.isNilGameObject(gameObj) then return end
	local tween = TweenManager.onDo(gameObj, tweening)

	if not TweenManager.tweenMap[gameObj] then
		TweenManager.tweenMap[gameObj] = {}
	end
	table.insert( TweenManager.tweenMap[gameObj], tween )

	return tween
end

-- 停止动画
-- complete 停止后是否直接跳到完成动画后的状态 默认false
function TweenManager.stopAllTweens( gameObj, complete )
	if Utils.isNilGameObject(gameObj) then return end
	local tweenList = TweenManager.tweenMap[gameObj]
	if tweenList then
		for i=#tweenList, 1, -1 do
			local tween = tweenList[i]
			tween:Kill(complete or false)
			tween = nil
			table.remove(tweenList,i)
		end
	end
	TweenManager.tweenMap[gameObj] = {}
end

function TweenManager._unpack( param )
	if Utils.isTable(param) then
		return unpack(param)
	end
	return param
end

function TweenManager.setCallBack(tween, callback, aFunc, ...)
	if Utils.isFunction(aFunc) then
		local args = {...}
		return tween[callback](tween, #args == 0 and aFunc or function()
			aFunc(unpack(args))
		end)
	end
	return tween
end

function TweenManager.setOnComplete(tween, aFunc, ...)
	return TweenManager.setCallBack(tween, 'OnComplete', aFunc, ...)
end

TweenManager.Param = {
	ease = function( tween, aEaseType )
		return tween:Ease(aEaseType)
	end,
	loop = function( tween, aTimes, aLoopType )
		return tween:Loop(aTimes, aLoopType or LoopType.Restart)
	end,
	delay = function( tween, aSecend )
		return tween:Delay(aSecend)
	end,
	from = function( tween )
		return tween:From()
	end,
	onStart = function( tween, ... )
		return TweenManager.setCallBack(tween, 'OnStart', ...)
	end,
	onLoop = function( tween, ... )
		return TweenManager.setCallBack(tween, 'OnStepComplete', ...)
	end,
	onEnd = function( tween, ... )
		return TweenManager.setOnComplete(tween, ...)
	end,
	onUpdate = function ( tween, ... )
		return TweenManager.setCallBack(tween, 'OnUpdate', ...)
	end
}

function TweenManager.noneTo( gameObj, duration )
	return DoTween.NoneTo(gameObj, duration or 0.0001)
end

TweenManager.Function = {
	anchorPosBy = function( gameObj, x, y, duration, ... )
		return TweenManager.setOnComplete(DoTween.AnchorPosBy(gameObj, Vector2(x,y), duration), ...)
	end,
	anchorPosTo = function( gameObj, x, y, duration, ... )
		return TweenManager.setOnComplete(DoTween.AnchorPosTo(gameObj, Vector2(x,y), duration), ...)
	end,
	anchorPosXBy = function( gameObj, x, duration, ... )
		return TweenManager.setOnComplete(DoTween.AnchorPosXBy(gameObj, x, duration), ...)
	end,
	anchorPosXTo = function( gameObj, x, duration, ... )
		return TweenManager.setOnComplete(DoTween.AnchorPosXTo(gameObj, x, duration), ...)
	end,
	anchorPosYBy = function( gameObj, y, duration, ... )
		return TweenManager.setOnComplete(DoTween.AnchorPosYBy(gameObj, y, duration), ...)
	end,
	anchorPosYTo = function( gameObj, y, duration, ... )
		return TweenManager.setOnComplete(DoTween.AnchorPosYTo(gameObj, y, duration), ...)
	end,
	localMoveBy = function( gameObj, x, y,z, duration, ... )
		return TweenManager.setOnComplete(DoTween.LocalMoveBy(gameObj, Vector3(x,y,z), duration), ...)
	end,
	localMoveTo = function( gameObj, x, y, z, duration, ... )
		return TweenManager.setOnComplete(DoTween.LocalMoveTo(gameObj, Vector3(x,y,z), duration), ...)
	end,
	moveBy = function( gameObj, x, y, z,duration, ... )
		return TweenManager.setOnComplete(DoTween.MoveBy(gameObj, Vector3(x,y,z), duration), ...)
	end,
	moveTo = function( gameObj, x, y, z,duration, ... )
		return TweenManager.setOnComplete(DoTween.MoveTo(gameObj, Vector3(x,y,z), duration), ...)
	end,
	rotateBy = function( gameObj, x, y, z, duration, ... )
		return TweenManager.setOnComplete(DoTween.RotateBy(gameObj,Vector3(x,y,z), duration), ...)
	end,
	rotateTo = function( gameObj, x, y, z, duration, ... )
		return TweenManager.setOnComplete(DoTween.RotateTo(gameObj, Vector3(x,y,z), duration), ...)
	end,
	rotateMBy = function( gameObj, x, y, z, mode, duration, ... )
		return TweenManager.setOnComplete(DoTween.RotateModeBy(gameObj, Vector3(x,y,z), mode or RotateMode.Fast, duration), ...)
	end,
	rotateMTo = function( gameObj,  x, y, z, mode, duration, ... )
		return TweenManager.setOnComplete(DoTween.RotateModeTo(gameObj, Vector3(x,y,z), mode or RotateMode.Fast, duration), ...)
	end,
	scaleBy = function( gameObj, x, y,z, duration, ... )
		return TweenManager.setOnComplete(DoTween.ScaleBy(gameObj, Vector3(x,y,z), duration), ...)
	end,
	scaleTo = function( gameObj, x, y,z, duration, ... )
		return TweenManager.setOnComplete(DoTween.ScaleTo(gameObj, Vector3(x,y,z), duration), ...)
	end,
	-- fadeTo -> fade with Graphic
	fadeTo = function( gameObj, alpha, duration, ... )
		return TweenManager.setOnComplete(DoTween.FadeTo(gameObj, alpha, duration), ...)
	end,
	-- fadeToCG -> fade with CanvasGroup
	fadeToCG = function( gameObj, alpha, duration, ... )
		return TweenManager.setOnComplete(DoTween.FadeToGroup(gameObj, alpha, duration), ...)
	end,
	colorTo = function( gameObj, r, g, b, a, duration, ... )
		return TweenManager.setOnComplete(DoTween.ColorTo(gameObj, Color(r, g, b, a), duration), ...)
	end,
	colorToAll = function( gameObj, r, g, b, a, duration, ... )
		return TweenManager.setOnComplete(DoTween.ColorToAll(gameObj, Color(r, g, b, a), duration), ...)
	end,
	delay = function( gameObj, delay, ... )
		return TweenManager.setOnComplete(TweenManager.noneTo(gameObj, delay), ...)
	end,
	call = function( gameObj, ... )
		return TweenManager.setOnComplete(TweenManager.noneTo(gameObj), ...)
	end,
	update = function( gameObj, duration, ... )
		return TweenManager.setCallBack(TweenManager.noneTo(gameObj, duration), 'OnUpdate', ...)
	end,
	to = function( gameObj, startValue, endValue, duration, setter, ... )
		return TweenManager.setOnComplete(DoTween.To(gameObj, startValue, endValue, duration, setter), ...)
	end,
	spawn = function( gameObj, ... )
		return TweenManager.sequenceTween( gameObj, {...}, "Insert" )
	end,
	preferredSizeBy = function ( gameObj, w, h, duration, snapping, ... )
		return TweenManager.setOnComplete(DoTween.PreferredSizeBy(gameObj, Vector2(w,h), duration, snapping or false), ...)
	end,
	preferredSizeTo = function ( gameObj, w, h, duration, snapping, ... )
		return TweenManager.setOnComplete(DoTween.PreferredSizeTo(gameObj, Vector2(w,h), duration, snapping or false), ...)
	end,
	-- ScrollRect 的方法
	horizontalNormalizedPosBy = function ( gameObj, x, duration, snapping, ... )
		return TweenManager.setOnComplete(DoTween.HorizontalNormalizedPosBy(gameObj, x, duration, snapping or false), ...)
	end,
	horizontalNormalizedPosTo = function ( gameObj, x, duration, snapping, ... )
		return TweenManager.setOnComplete(DoTween.HorizontalNormalizedPosTo(gameObj, x, duration, snapping or false), ...)
	end,
	verticalNormalizedPosBy = function ( gameObj, y, duration, snapping, ... )
		return TweenManager.setOnComplete(DoTween.VerticalNormalizedPosBy(gameObj, y, duration, snapping or false), ...)
	end,
	verticalNormalizedPosTo = function ( gameObj, y, duration, snapping, ... )
		return TweenManager.setOnComplete(DoTween.VerticalNormalizedPosTo(gameObj, y, duration, snapping or false), ...)
	end,
	normalizedPosBy = function ( gameObj, x, y, duration, snapping, ... )
		return TweenManager.setOnComplete(DoTween.NormalizedPosBy(gameObj, Vector2(x, y), duration, snapping or false), ...)
	end,
	normalizedPosTo = function ( gameObj, x, y, duration, snapping, ... )
		return TweenManager.setOnComplete(DoTween.NormalizedPosTo(gameObj, Vector2(x, y), duration, snapping or false), ...)
    end,
    punchPosition = function( gameObj, x, y, z, duration,vibrato,elascity, ... )
		return TweenManager.setOnComplete(DoTween.DOPunchPosition(gameObj, Vector3(x,y,z), duration, vibrato or 10, elascity or 1), ...)
    end,
    punchScale = function( gameObj, x, y, z, duration,vibrato,elascity, ... )
		return TweenManager.setOnComplete(DoTween.DOPunchScale(gameObj, Vector3(x,y,z), duration, vibrato or 10, elascity or 1), ...)
    end,
    punchRotation = function( gameObj, x, y, z, duration,vibrato,elascity, ... )
		return TweenManager.setOnComplete(DoTween.DOPunchRotation(gameObj, Vector3(x,y,z), duration, vibrato or 10, elascity or 1), ...)
	end
}

function TweenManager.setParam( tween, tweening, funcName )
	if tween ~= nil then
		for name, param in pairs(tweening) do
			if Utils.isString(name) then
				Utils.safeCallFunc(TweenManager.Param[name], tween, TweenManager._unpack(param))
			end
		end
	else
		logError("tweening not find : "..funcName)
	end
	return tween
end

function TweenManager.sequenceTween( gameObj, tweening, opt )
	local sequence = DoTween.Sequence(gameObj)
	local i = 0;
	for _, act in ipairs(tweening) do
		if Utils.isTable(act) then
			sequence[opt](sequence, TweenManager.onDo(gameObj, act))
		end
		i = i + 1
	end
	return TweenManager.setParam(sequence, tweening)
end

function TweenManager.onDo( gameObj, tweening )
	return TweenManager.createTween( gameObj, tweening, unpack(tweening) )
end

function TweenManager.createTween( gameObj, tweening, funcName, ... )
	if Utils.isString(funcName) then
		return TweenManager.setParam( Utils.safeDoFunc(TweenManager.Function[funcName], gameObj, ...), tweening, funcName )
	elseif Utils.isTable(funcName) then
		return TweenManager.sequenceTween( gameObj, tweening, "Append" )
	elseif Utils.isFunction(funcName) then
		return TweenManager.Function.call(gameObj, funcName, ...)
	end
end

return TweenManager
