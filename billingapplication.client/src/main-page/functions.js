export const timeToMinutes = (time) => {
    if (time === undefined) {
        return '';
    }
    const [hours, minutes, seconds] = time.split(":").map(Number);
    return hours * 60 + minutes + Math.floor(seconds / 60);
};

export const getMaxValue = (currentValue, maxValue) => {
    return Math.max(currentValue, maxValue);
}

export const getRandomObjects = (array, count) => {
    if (count >= array.length) {
        return [...array]; 
    }

    const shuffled = [...array].sort(() => Math.random() - 0.5);
    return shuffled.slice(0, count);
};

export const getObjectById = (array, id) => {
    return array.find(item => item.id === id) || null;
}

export const getTypeExtra = (bundle) => {
    if (bundle.callTime && bundle.callTime !== "00:00:00") {
        const [hours, minutes, seconds] = bundle.callTime.split(":").map(Number);
        const totalMinutes = hours * 60 + minutes + seconds / 60;
        return `${Math.round(totalMinutes)} минут`;
    }

    if (bundle.messages > 0) {
        return `${bundle.messages} смс`;
    }

    if (bundle.internet > 0) {
        return `${bundle.internet / 1024} гб`;
    }
};