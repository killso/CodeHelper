## -*- coding: utf-8 -*-

import speech_recognition as speech_r
import pyaudio
import wave
import sys
import codecs
import winsound

sys.stdout = codecs.getwriter('utf-16')(sys.stdout.detach())

CHUNK = 1024 # определяет форму ауди сигнала
FRT = pyaudio.paInt16 # шестнадцатибитный формат задает значение амплитуды
CHAN = 1 # канал записи звука
RT = 44100 # частота 
REC_SEC = 5 #длина записи
OUTPUT = "output.wav"

p = pyaudio.PyAudio()

winsound.PlaySound("gong1.wav", winsound.SND_FILENAME)

stream = p.open(format=FRT,channels=CHAN,rate=RT,input=True,frames_per_buffer=CHUNK) # открываем поток для записи
#recording start
print("rec started")
frames = [] # формируем выборку данных фреймов
for i in range(0, int(RT / CHUNK * REC_SEC)):   
    data = stream.read(CHUNK)
    frames.append(data)
#recording done
stream.stop_stream() # останавливаем и закрываем поток
stream.close()
p.terminate()

w = wave.open(OUTPUT, 'wb')
w.setnchannels(CHAN)
w.setsampwidth(p.get_sample_size(FRT))
w.setframerate(RT)
w.writeframes(b''.join(frames))
w.close()

sample = speech_r.WavFile('output.wav')

r = speech_r.Recognizer()

with sample as audio:
    content = r.record(audio)

print(r.recognize_google(content, language="ru-RU"))

A_Data = 'output.wav'
