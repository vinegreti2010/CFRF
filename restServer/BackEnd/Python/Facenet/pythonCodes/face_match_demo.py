import tensorflow as tf
import numpy as np
import facenet
from align import detect_face
import cv2
import argparse
import threading

parser = argparse.ArgumentParser()
parser.add_argument("--img1", type = str, required=True)
parser.add_argument("--img2", type = str, required=True)
parser.add_argument("--img3", type = str, required=True)
parser.add_argument("--img4", type = str, required=True)
args = parser.parse_args()

# some constants kept as default from facenet
minsize = 20
threshold = [0.6, 0.7, 0.7]
factor = 0.709
margin = 44
input_image_size = 160

sess = tf.Session()

# read pnet, rnet, onet models from align directory and files are det1.npy, det2.npy, det3.npy
pnet, rnet, onet = detect_face.create_mtcnn(sess, 'C:/Users/Administrador/Documents/Visual Studio 2017/Projects/restServer/restServer/restServer/BackEnd/Python/Facenet/pythonCodes/align')

# read 20170512-110547 model file downloaded from https://drive.google.com/file/d/0B5MzpY9kBtDVZ2RpVDYwWmxoSUk
#facenet.load_model("D:/home/site/wwwroot/BackEnd/Python/Facenet/models/20170512-110547.pb")
facenet.load_model("C:/Users/Administrador/Documents/Visual Studio 2017/Projects/restServer/restServer/restServer/BackEnd/Python/Facenet/models/20170512-110547.pb")

# Get input and output tensors
images_placeholder = tf.get_default_graph().get_tensor_by_name("input:0")
embeddings = tf.get_default_graph().get_tensor_by_name("embeddings:0")
phase_train_placeholder = tf.get_default_graph().get_tensor_by_name("phase_train:0")
embedding_size = embeddings.get_shape()[1]

faces = []
facesToCompare = []

class execOnThread(threading.Thread):
	def __init__(self, img, index, minsize, pnet, rnet, onet, threshold, factor):
		threading.Thread.__init__(self)
		self.img = img
		self.index = index
		self.minsize = minsize
		self.pnet = pnet
		self.rnet = rnet
		self.onet = onet
		self.threshold = threshold
		self.factor = factor
	def run(self):
		facesDetected = getFace(img, minsize, pnet, rnet, onet, threshold, factor)
		threadLock.acquire()
		if(self.index < 2):
			facesToCompare.append(facesDetected[0]['embedding'])
		else:
			faces.append(facesDetected[0]['embedding'])
		threadLock.release()

def getFace(img, minsize, pnet, rnet, onet, threshold, factor):
    facesDetected = []
    img_size = np.asarray(img.shape)[0:2]
    bounding_boxes, _ = detect_face.detect_face(img, minsize, pnet, rnet, onet, threshold, factor)
    if not len(bounding_boxes) == 0:
        for face in bounding_boxes:
            if face[4] > 0.50:
                det = np.squeeze(face[0:4])
                bb = np.zeros(4, dtype=np.int32)
                bb[0] = np.maximum(det[0] - margin / 2, 0)
                bb[1] = np.maximum(det[1] - margin / 2, 0)
                bb[2] = np.minimum(det[2] + margin / 2, img_size[1])
                bb[3] = np.minimum(det[3] + margin / 2, img_size[0])
                cropped = img[bb[1]:bb[3], bb[0]:bb[2], :]
                resized = cv2.resize(cropped, (input_image_size,input_image_size),interpolation=cv2.INTER_CUBIC)
                prewhitened = facenet.prewhiten(resized)
                facesDetected.append({'face':resized,'rect':[bb[0],bb[1],bb[2],bb[3]],'embedding':getEmbedding(prewhitened)})
    return facesDetected

def getEmbedding(resized):
    reshaped = resized.reshape(-1,input_image_size,input_image_size,3)
    feed_dict = {images_placeholder: reshaped, phase_train_placeholder: False}
    embedding = sess.run(embeddings, feed_dict=feed_dict)
    return embedding

def compare2face(face1,face2):
    if np.any(face1) and np.any(face2):
        # calculate Euclidean distance
        dist = np.sqrt(np.sum(np.square(np.subtract(face1, face2))))
        return dist
    return -1

def isPhotoOfPhoto(face1, face2, face3):
	if np.any(face1) and np.any(face2) and np.any(face3):
		if np.array_equal(face1[0], face2[0]) and np.array_equal(face1[0], face3[0]):
			return -2
		else:
			return 0
	else:
		return -1
		
#img1 = cv2.imread(args.img1)
#img2 = cv2.imread(args.img2)
#img3 = cv2.imread(args.img3)
#img4 = cv2.imread(args.img4)

#face1 = getFace(img1)
#face2 = getFace(img2)
#face3 = getFace(img3)
#face4 = getFace(img4)

images = []

images.append(cv2.imread(args.img1))
images.append(cv2.imread(args.img2))
images.append(cv2.imread(args.img3))
images.append(cv2.imread(args.img4))

threadLock = threading.Lock()
threads = []
index = 0
for img in images:
	faceThread = execOnThread(img, index, minsize, pnet, rnet, onet, threshold, factor)
	faceThread.start()
	threads.append(faceThread)
	index += 1

for t in threads:
    t.join()

photoOfPhoto = isPhotoOfPhoto(facesToCompare[1], faces[0], faces[1])
if photoOfPhoto == 0:
	distance = compare2face(facesToCompare[0], facesToCompare[1])
	print(str(distance) + str(len(facesToCompare)))
else:
	print(str(photoOfPhoto))