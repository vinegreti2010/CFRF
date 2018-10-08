from flask import jsonify
from flask_restful import reqparse
from FacenetServer import FacenetMatch
from FacenetServer import app
import time

@app.route('/facenet', methods=['GET'])
def teste():
    return jsonify({'teste':'fdfs'})

@app.route('/facenet/recognizeFaces', methods=['POST'])
def recognizeFaces():
    parser = reqparse.RequestParser()
    parser.add_argument("baseImage")
    parser.add_argument("img1")
    parser.add_argument("img2")
    parser.add_argument("img3")

    args = parser.parse_args()

    images = [args.img1, args.img2, args.img3]
    start = time.time()
    distance = FacenetMatch.recognize(images, args.baseImage)
    elapsed_time = time.time() - start;
    return jsonify({'distance': str(distance), 'time':str(elapsed_time)})

@app.route('/facenet/generateEmbedding', methods=['POST'])
def generateEmbedding():
    parser = reqparse.RequestParser()
    parser.add_argument("img")

    args = parser.parse_args()

    embedding = FacenetMatch.generateEmbedding(args.img)

    return jsonify({'embedding': embedding.toList()})