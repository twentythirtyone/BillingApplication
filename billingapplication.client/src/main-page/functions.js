export const timeToMinutes = (time) => {
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