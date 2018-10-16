import tensorflow as tf
import numpy as np
from FacenetServer import facenet
from .align import detect_face
import cv2
import argparse
import base64

# some constants kept as default from facenet
minsize = 20
threshold = [0.6, 0.7, 0.7]
factor = 0.709
margin = 44
input_image_size = 160

sess = tf.Session()

# read pnet, rnet, onet models from align directory and files are det1.npy, det2.npy, det3.npy
pnet, rnet, onet = detect_face.create_mtcnn(sess, 'D:/home/site/wwwroot/FacenetServer/align')
#pnet, rnet, onet = detect_face.create_mtcnn(sess, 'C:/Users/Administrador/Documents/Visual Studio 2017/Projects/restServer/restServer/FacenetServer/FacenetServer/align')

# read 20170512-110547 model file downloaded from https://drive.google.com/file/d/0B5MzpY9kBtDVZ2RpVDYwWmxoSUk
facenet.load_model("D:/home/site/wwwroot/FacenetServer/models/20170512-110547.pb")
#facenet.load_model("C:/Users/Administrador/Documents/Visual Studio 2017/Projects/restServer/restServer/FacenetServer/FacenetServer/models/20170512-110547.pb")

# Get input and output tensors
images_placeholder = tf.get_default_graph().get_tensor_by_name("input:0")
embeddings = tf.get_default_graph().get_tensor_by_name("embeddings:0")
phase_train_placeholder = tf.get_default_graph().get_tensor_by_name("phase_train:0")
embedding_size = embeddings.get_shape()[1]


def getFace(img):
    faces = []
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
                resized = cv2.resize(cropped, (input_image_size, input_image_size), interpolation=cv2.INTER_CUBIC)
                prewhitened = facenet.prewhiten(resized)
                faces.append(
                    {'face': resized, 'rect': [bb[0], bb[1], bb[2], bb[3]], 'embedding': getEmbedding(prewhitened)})
    return faces


def getEmbedding(resized):
    reshaped = resized.reshape(-1, input_image_size, input_image_size, 3)
    feed_dict = {images_placeholder: reshaped, phase_train_placeholder: False}
    embedding = sess.run(embeddings, feed_dict=feed_dict)
    return embedding


def isPhotoOfPhoto(images):
    comp1 = compareFaces(images[0], images[1])
    comp2 = compareFaces(images[0], images[2])
    resp = 0

    if comp1 < 0 or comp2 < 0:
        resp = -1
    elif comp1 < 0.07 and comp2 < 0.07:
        resp = -2

    del comp1
    del comp2

    return resp


def compareFaces(img1, img2):
    face1 = getFace(img1)
    face2 = getFace(img2)

    if face1 and face2:
        dist = np.sqrt(np.sum(np.square(np.subtract(face1[0]['embedding'], face2[0]['embedding']))))
        del face1
        del face2
        return dist
    return -1


def recognize(imagesBase64, baseImageBase64):
    baseImage = base64ToImage(baseImageBase64)
    images = []
    for image in imagesBase64:
        images.append(base64ToImage(image))
    photoOfPhoto = isPhotoOfPhoto(images)
    if(photoOfPhoto == 0):
        result = compareFaces(baseImage, images[0])
    else:
        result = photoOfPhoto

    del baseImage
    images.clear()

    return result


def generateEmbedding(image):
    img = cv2.imread(image)
    face = getFace(img)
    return face[0]['embedding']


def base64ToImage(imageEncoded):
    imgbytes = bytes(imageEncoded, 'UTF-8')
    imageDecoded = base64.decodebytes(imgbytes)
    imgArray = np.fromstring(imageDecoded, np.uint8)
    img = cv2.imdecode(imgArray, cv2.IMREAD_COLOR)
    imgPortrait = toPortrait(img)
    return imgPortrait

def toPortrait(img):
    imgResult = img
    img_size = np.asarray(img.shape)[0:2]

    if img_size[0] < img_size[1]:
        M = cv2.getRotationMatrix2D((img_size[1]/2,img_size[0]/2),90,1)
        imgResult = cv2.warpAffine(img,M,(img_size[1],img_size[0]))

    return imgResult


def hasFace(imageBase64):
    img = base64ToImage(imageBase64)
    face1 = getFace(img)

    if face1:
        return 1;

    return 0;